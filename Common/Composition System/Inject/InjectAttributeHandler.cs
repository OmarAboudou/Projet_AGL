using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Godot;

namespace Composition.Composition_System.Inject;

[Tool]
public abstract partial class InjectAttributeHandler : Node
{
    private readonly List<Node> _handledNodes = new();
    protected abstract Type AttributeType { get; }
    
    public override void _EnterTree()
    {
        base._EnterTree();
        this.GetTree().NodeAdded += this.OnNodeAdded;
        this.GetTree().NodeRemoved += this.OnNodeRemoved;
        if (!Engine.IsEditorHint())
        {
            this.GetTree().TreeChanged += this.InjectAll;
        }
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        this.GetTree().NodeAdded -= this.OnNodeAdded;
        this.GetTree().NodeRemoved -= this.OnNodeRemoved;
        if (!Engine.IsEditorHint())
        {
            this.GetTree().TreeChanged -= this.InjectAll;
        }
    }

    protected abstract List<Node> GetInjectionCandidates(Node injected);
    
    private void UpdateInjection(Node injected)
    {
        Type nodeType = this.GetNodeType(injected);
        List<Node> candidates = this.GetInjectionCandidates(injected);

        this.UpdateFieldsInjection(injected, nodeType, candidates);
        this.UpdatePropertiesInjection(injected, nodeType, candidates);
    }
    
    private void UpdateFieldsInjection(Node injected, Type nodeType, List<Node> candidates)
    {
        List<FieldInfo> injectedFieldInfos = this.GetInjectedFields(nodeType);
        foreach (FieldInfo injectedFieldInfo in injectedFieldInfos)
        {
            List<Node> validCandidates = 
                candidates
                    .Where(c => this.GetNodeType(c).IsAssignableTo(injectedFieldInfo.FieldType))
                    .ToList();

            if (validCandidates.Count <= 0)
            {
                GD.PrintErr($"Couldn't find any candidate for field {injected.Name}.{injectedFieldInfo.Name}");
                this.ClearField(injected, injectedFieldInfo);

            }
            else if (validCandidates.Count > 1)
            {
                GD.PrintErr($"Multiple candidates for field {injected.Name}.{injectedFieldInfo.Name}");
                this.ClearField(injected, injectedFieldInfo);
            }
            else
            {
                Node injection = validCandidates[0];
                this.SetField(injected, injection, injectedFieldInfo);
                // GD.Print($"Injected {injection.Name} into field {injected.Name}.{injectedFieldInfo.Name}");
            }
        }
    }

    private void UpdatePropertiesInjection(Node injected, Type nodeType, List<Node> candidates)
    {
        List<PropertyInfo> injectedPropertyInfos = this.GetInjectedProperties(nodeType);
        foreach (PropertyInfo injectedPropertyInfo in injectedPropertyInfos)
        {
            List<Node> validCandidates = 
                candidates
                    .Where(c => this.GetNodeType(c).IsAssignableTo(injectedPropertyInfo.PropertyType))
                    .ToList();

            if (validCandidates.Count <= 0)
            {
                GD.PrintErr($"Couldn't find any candidate for property {injected.Name}.{injectedPropertyInfo.Name}");
                this.ClearProperty(injected, injectedPropertyInfo);
            }
            else if (validCandidates.Count > 1)
            {
                GD.PrintErr($"Multiple candidates for property {injected.Name}.{injectedPropertyInfo.Name}");
                this.ClearProperty(injected, injectedPropertyInfo);
            }
            else
            {
                Node injection = validCandidates[0];
                this.SetProperty(injected, injection, injectedPropertyInfo);
                GD.Print($"Injected {injection.Name} into property {injected.Name}.{injectedPropertyInfo.Name}");
            }
        }
    }
    
    private void SetField(Node injected, Node injection, FieldInfo fieldInfo)
    {
        if (!Engine.IsEditorHint())
        {
            fieldInfo.SetValue(injected, injection);
        }
        else
        {
            injected.Set(fieldInfo.Name, injection);
        }
    }

    private void ClearField(Node injected, FieldInfo fieldInfo)
    {
        if (!Engine.IsEditorHint())
        {
            fieldInfo.SetValue(injected, null);
        }
        else
        {
            injected.Set(fieldInfo.Name, default);
        }
    }

    private void SetProperty(Node injected, Node injection, PropertyInfo propertyInfo)
    {
        if (!Engine.IsEditorHint())
        {
            propertyInfo.SetValue(injected, injection);
        }
        else
        {
            injected.Set(propertyInfo.Name, injection);
        }
    }

    private void ClearProperty(Node injected, PropertyInfo propertyInfo)
    {
        if (!Engine.IsEditorHint())
        {
            propertyInfo.SetValue(injected, null);
        }
        else
        {
            injected.Set(propertyInfo.Name, default);
        }
    }
    
    private void OnNodeAdded(Node node)
    {
        if (Engine.IsEditorHint())
        {
            if (node.IsPartOfEditedScene())
            {
                node.ChildOrderChanged += this.InjectAll;
            }
        }
        
        if(!this.IsHandled(node)) return;
        
        this._handledNodes.Add(node);
        this.UpdateInjection(node);
    }

    private void OnNodeRemoved(Node node)
    {
        if (Engine.IsEditorHint())
        {
            if (node.IsPartOfEditedScene())
            {
                node.ChildOrderChanged -= this.InjectAll;
            }
        }
        
        if(this._handledNodes.Contains(node))
        {
            this._handledNodes.Remove(node);
        }
    }
    
    private void InjectAll()
    {
        foreach (Node handledNode in this._handledNodes)
        {
            this.UpdateInjection(handledNode);
        }
    }
    
    private bool IsHandled(Node node)
    {
        Type nodeType = this.GetNodeType(node);

        return this.HasAnyInjectedFields(nodeType) || this.HasAnyInjectedProperties(nodeType);
    }

    private Type GetNodeType(Node node)
    {
        Type nodeType = node.GetType();
        if (!Engine.IsEditorHint()) return nodeType;
        
        Type scriptType = (node.GetScript().Obj as Script)?.GetScriptType();
        if(scriptType != null) return scriptType;
        
        return nodeType;
    }

    private List<FieldInfo> GetInjectedFields(Type type)
    {
        return type
            .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            .Where(info => info
                .GetCustomAttributes()
                .Any(attribute => attribute.GetType() == this.AttributeType))
            .ToList();
    }

    private List<PropertyInfo> GetInjectedProperties(Type type)
    {
        return type
            .GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            .Where(info => info
                .GetCustomAttributes()
                .Any(attribute => attribute.GetType() == this.AttributeType))
            .ToList();
    }
    
    private bool HasAnyInjectedFields(Type type)
    {
        return GetInjectedFields(type).Any();
    }

    private bool HasAnyInjectedProperties(Type type)
    {
        return GetInjectedProperties(type).Any();
    }
    
}