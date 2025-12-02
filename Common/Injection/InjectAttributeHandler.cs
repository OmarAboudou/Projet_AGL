using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Common.Injection.Inject_Attributes;
using Common.Log;
using Common.Utils;
using Godot;

namespace Common.Injection;

[Tool]
public sealed partial class InjectAttributeHandler : Node
{
    private readonly List<Node> _handledNodes = new();
    private bool _injectAllQueued;
    private Callable _scheduleInjectAllCallable;

    public override void _Ready()
    {
        base._Ready();
        // this.SetLogEnabled(LogType.INFO, true);
        this.SetLogEnabled(LogType.WARNING, true);
        this.SetLogEnabled(LogType.ERROR, true);
        this.SetLogEnabled(LogType.CRITICAL, true);
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        this._scheduleInjectAllCallable = Callable.From(this.ScheduleInjectAll);
        this.GetTree().NodeAdded += this.OnNodeAdded;
        this.GetTree().NodeRemoved += this.OnNodeRemoved;
    }
    
    public override void _ExitTree()
    {
        base._ExitTree();
        this.GetTree().NodeAdded -= this.OnNodeAdded;
        this.GetTree().NodeRemoved -= this.OnNodeRemoved;

    }
    
    private void ScheduleInjectAll()
    {
        if (this._injectAllQueued)
            return;

        this._injectAllQueued = true;
        this.CallDeferred(nameof(this.DeferredInjectAll));
    }

    private void DeferredInjectAll()
    {
        this._injectAllQueued = false;
        this.InjectAll();
    }

    
    private void UpdateInjection(Node injected)
    {
        Type nodeType = this.GetNodeType(injected);

        List<MemberInfo> injectedMemberInfos = this.GetInjectedMembers(nodeType);
        foreach (MemberInfo injectedMemberInfo in injectedMemberInfos)
        {
            this.Inject(injected, injectedMemberInfo);
        }
        
    }
    
    private void Inject(Node injected, MemberInfo injectedMemberInfo)
    {
        ImmutableArray<InjectAttribute> injectAttributes = GetInjectAttributes(injectedMemberInfo);
        if (injectAttributes.Length == 0)
            return;

        List<Node> candidates = this.BuildCandidates(injected, injectAttributes);

        Type memberType = this.GetMemberType(injectedMemberInfo);

        bool isCollection = TryGetCollectionElementType(memberType, out Type elementType);
        List<Node> validCandidates = candidates
            .Where(c => this.GetNodeType(c).IsAssignableTo(isCollection ? elementType : memberType))
            .ToList();
            
        if (isCollection)
        {
            this.InjectCollection(injected, injectedMemberInfo, elementType, validCandidates);
        }
        else
        {
            this.InjectSingle(injected, injectedMemberInfo, memberType, validCandidates);
        }
    }

    private static ImmutableArray<InjectAttribute> GetInjectAttributes(MemberInfo member)
    {
        return [..member.GetCustomAttributes<InjectAttribute>()];
    }

    private List<Node> BuildCandidates(Node injected, ImmutableArray<InjectAttribute> injectAttributes)
    {
        List<Node> candidates = new List<Node>();
        if (injectAttributes.Length == 0)
            return candidates;

        int index = 0;

        while (index < injectAttributes.Length)
        {
            InjectAttribute injectAttribute = injectAttributes[index];
            List<Node> newCandidates   = injectAttribute.ProcessAttributes(injected, ref index, injectAttributes);

            foreach (Node newCandidate in newCandidates)
            {
                if (newCandidate == null)
                    continue;

                if (candidates.Contains(newCandidate))
                    continue;

                if (!IsCandidateAllowedInEditor(newCandidate))
                    continue;

                candidates.Add(newCandidate);
            }

            index++;
        }

        return candidates;
    }

    private static bool IsCandidateAllowedInEditor(Node newCandidate)
    {
        if (!Engine.IsEditorHint())
            return true;

        if (newCandidate.IsPartOfEditedScene())
            return true;

        Node owner = newCandidate.GetOwner();
        return owner != null && owner.IsPartOfEditedScene();
    }

    private static bool TryGetCollectionElementType(Type memberType, out Type elementType)
    {
        elementType = null;

        // Le membre est directement ICollection<T>
        if (memberType.IsGenericType &&
            memberType.GetGenericTypeDefinition() == typeof(ICollection<>))
        {
            elementType = memberType.GetGenericArguments()[0];
            return true;
        }

        // Le membre implémente ICollection<T> (List<T>, HashSet<T>, etc.)
        Type iCollectionInterface = memberType
            .GetInterfaces()
            .FirstOrDefault(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(ICollection<>));

        if (iCollectionInterface != null)
        {
            elementType = iCollectionInterface.GetGenericArguments()[0];
            return true;
        }

        return false;
    }

    private void InjectCollection(
        Node injected,
        MemberInfo injectedMemberInfo,
        Type elementType,
        List<Node> validCandidates)
    {
        this.Log(LogType.INFO, $"{injectedMemberInfo.Name} is collection type {injectedMemberInfo.MemberType} " +
                                             $"of elements of type : {elementType.Name}.");
        
        object collectionInstance = this.GetMemberValue(injected, injectedMemberInfo);

        if (collectionInstance == null)
        {
            this.Log(LogType.ERROR,$"Collection {injectedMemberInfo.MemberType} : {injectedMemberInfo.Name} must be initialized for injection to work.");
            return;
        }

        Type collectionType = collectionInstance.GetType();
        
        collectionType
            .GetMethod(nameof(ICollection<int>.Clear))
            ?.Invoke(collectionInstance, []);
        
        MethodInfo addMethod = collectionType
            .GetMethod(nameof(ICollection<int>.Add), BindingFlags.Public | BindingFlags.Instance);

        foreach (Node validCandidate in validCandidates)
        {
            object transformedValidCandidate;

            if (collectionType == typeof(Godot.Collections.Array))
            {
                transformedValidCandidate = Variant.CreateFrom(validCandidate);
            }
            else
            {
                transformedValidCandidate = validCandidate;
            }

            addMethod?.Invoke(collectionInstance, [transformedValidCandidate]);
        }
    }

    private void InjectSingle(
        Node injected,
        MemberInfo injectedMemberInfo,
        Type validCandidateType,
        List<Node> validCandidates)
    {
        MemberTypes memberKind = injectedMemberInfo.MemberType; // Field / Property

        if (validCandidates.Count == 0)
        {
            this.Log(LogType.ERROR,$"Couldn't find any candidate for {memberKind} {injected.Name}.{injectedMemberInfo.Name}");
            this.ClearMemberValue(injected, injectedMemberInfo);
            return;
        }

        if (validCandidates.Count > 1)
        {
            this.Log(LogType.WARNING,$"Multiple candidates for {memberKind} {injected.Name}.{injectedMemberInfo.Name}");
        }

        Node injection = validCandidates[0];
        this.SetMemberValue(injected, injection, injectedMemberInfo);
        
        this.Log(LogType.INFO,$"Injected {injection.Name} into {memberKind} {injected.Name}.{injectedMemberInfo.Name}");
    }


    private Type GetMemberType(MemberInfo memberInfo)
    {
        return memberInfo switch
        {
            FieldInfo fieldInfo => fieldInfo.FieldType,
            PropertyInfo propertyInfo => propertyInfo.PropertyType,
            _ => throw new InvalidOperationException($"MemberInfo must be of type FieldInfo or PropertyInfo.")
        };
    }
    
    private void OnNodeAdded(Node node)
    {
        if(Engine.IsEditorHint() && !node.IsPartOfEditedScene()) return;
        
        this._handledNodes.Add(node);
        
        Node owner = node.GetOwner();
        if (owner == null || owner.IsNodeReady())
        {
            this.ScheduleInjectAll();
        }
        else
        {
            if (owner.IsConnected(Node.SignalName.Ready, this._scheduleInjectAllCallable)) return;
            
            owner.Ready += OwnerOnReady;
            void OwnerOnReady()
            {
                owner.Ready -= OwnerOnReady;
                this.ScheduleInjectAll();
            }
            
        }
    }

    private void OnNodeRemoved(Node node)
    {
        if(Engine.IsEditorHint() && !node.IsPartOfEditedScene()) return;
        
        if(this._handledNodes.Contains(node))
        {
            this._handledNodes.Remove(node);
            this.ScheduleInjectAll();
        }
    }
    
    private void InjectAll()
    {
        foreach (Node handledNode in this._handledNodes)
        {
            this.UpdateInjection(handledNode);
        }
    }
    
    private List<MemberInfo> GetInjectedMembers(Type type)
    {
        List<MemberInfo> members = 
            type
            .GetMembers(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            .Where
            (
                info => info
                    .GetCustomAttributes<InjectAttribute>()
                    .Any()
            ).ToList();
        return members;
    }
    
    private Type GetNodeType(Node node)
    {
        Type nodeType = node.GetType();
        if (!Engine.IsEditorHint()) return nodeType;
        
        Type scriptType = (node.GetScript().Obj as Script)?.GetScriptType();
        if(scriptType != null) return scriptType;
        
        return nodeType;
    }

    private object GetMemberValue(Node node, MemberInfo memberInfo)
    {
        object value = null;
        if (!Engine.IsEditorHint())
        {
            value = memberInfo switch
            {
                FieldInfo fieldInfo => fieldInfo.GetValue(node),
                PropertyInfo propertyInfo => propertyInfo.GetValue(node),
                _ => throw new InvalidOperationException("MemberInfo must be of type FieldInfo or PropertyInfo")
            };    
        }
        else
        {
            value = node.Get(memberInfo.Name).Obj;
        }
        
        return value;
    }

    private void SetMemberValue(Node node, Variant value, MemberInfo memberInfo)
    {
        if (!Engine.IsEditorHint())
        {
            switch (memberInfo)
            {
                case FieldInfo fieldInfo:
                    fieldInfo.SetValue(node, value.Obj);
                    break;
                case PropertyInfo propertyInfo:
                    propertyInfo.SetValue(node, value.Obj);
                    break;
                default:
                    throw new InvalidOperationException("MemberInfo must be of type FieldInfo or PropertyInfo");
            }    
        }
        else
        {
            node.Set(memberInfo.Name, value);
        }
    }

    private void ClearMemberValue(Node node, MemberInfo memberInfo)
    {
        if(!Engine.IsEditorHint())
        {
            switch (memberInfo)
            {
                case FieldInfo fieldInfo:
                    fieldInfo.SetValue(node, null);
                    break;
                case PropertyInfo propertyInfo:
                    propertyInfo.SetValue(node, null);
                    break;
                default:
                    throw new InvalidOperationException("MemberInfo must be of type FieldInfo or PropertyInfo");
            }
        }
        else
        {
            node.Set(memberInfo.Name, default);
        }
    }
}