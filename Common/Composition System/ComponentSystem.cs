using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Composition.Utils;
using Godot;
using Godot.Collections;

namespace Composition.Composition_System;

[Tool]
public partial class ComponentSystem : Node
{
    private const string DependantNodesGroupName = "DependantNodes";
    private const string ParentOfDependantsGroupName = "ParentOfDependantNodes";
    
    public override void _EnterTree()
    {
        base._EnterTree();
        this.GetTree().NodeAdded += this.OnNodeAdded;
    }

    private void UpdateDependencies(Node dependant)
    {
        Node parent = dependant.GetParent();
        if(parent == null) return;
        
        Type dependantType = dependant.GetType();
        if (Engine.IsEditorHint())
        {
            dependantType = (dependant.GetScript().Obj as Script)?.GetScriptType();
        }
        if(dependantType == null) return;
        
        Type parentType = parent.GetType();
        if (Engine.IsEditorHint())
        {
            parentType = (parent.GetScript().Obj as Script)?.GetScriptType() ?? parent.GetType();
        }

        IEnumerable<FieldInfo> injectParentFieldInfos = GetInjectParentFieldInfo(dependantType);
        foreach (FieldInfo injectParentFieldInfo in injectParentFieldInfos)
        {
            TryInjectParentField(dependant, injectParentFieldInfo, parent, parentType);
        }
        
        IEnumerable<FieldInfo> injectFieldInfos = GetInjectFieldInfos(dependantType);
        Array<Node> siblings = parent.GetChildren();
        
        foreach (FieldInfo injectFieldInfo in injectFieldInfos)
        {
            TryInjectField(dependant, injectFieldInfo, siblings);
        }
        
        IEnumerable<PropertyInfo> injectParentPropertyInfos = GetInjectParentPropertyInfos(dependantType);
        foreach (PropertyInfo injectParentPropertyInfo in injectParentPropertyInfos)
        {
            this.TryInjectParentProperty(dependant, injectParentPropertyInfo, parent, parentType);
        }
        
        IEnumerable<PropertyInfo>  injectPropertyInfos = GetInjectPropertyInfos(dependantType);

        foreach (PropertyInfo injectPropertyInfo in injectPropertyInfos)
        {
            TryInjectProperty(dependant, injectPropertyInfo, siblings);
        }
        
        this.UpdateParentInGroup(parent);
    }

    private void TryInjectParentProperty(Node dependant, PropertyInfo injectParentPropertyInfo, Node parent, Type parentType)
    {
        if (injectParentPropertyInfo.PropertyType.IsAssignableFrom(parentType))
        {
            dependant.Set(injectParentPropertyInfo.Name, parent);
        }
        else
        {
            GD.PrintErr($"{dependant.Name} requires a parent of type {injectParentPropertyInfo.PropertyType.Name}");
        }
    }
    private void TryInjectParentField(Node dependant, FieldInfo injectParentFieldInfo, Node parent, Type parentType)
    {
        if (injectParentFieldInfo.FieldType.IsAssignableFrom(parentType))
        {
            dependant.Set(injectParentFieldInfo.Name, parent);
        }
        else
        {
            GD.PrintErr($"{dependant.Name} requires a parent of type {injectParentFieldInfo.FieldType.Name}");
        }
    }


    private static void TryInjectProperty(Node dependant, PropertyInfo injectPropertyInfo, Array<Node> siblings)
    {
        dependant.AddToGroup(DependantNodesGroupName);
        Type injectPropertyType = injectPropertyInfo.PropertyType;
        List<Node> validCandidatesForInjection = new();
        foreach (Node sibling in siblings)
        {
            Type siblingType = sibling.GetType();
            if (Engine.IsEditorHint())
            {
                siblingType = (sibling.GetScript().Obj as Script)?.GetScriptType();
            }
            if(siblingType == null) continue;
            
            if (siblingType.IsAssignableTo(injectPropertyType))
            {
                validCandidatesForInjection.Add(sibling);
            }
        }
            
        if (validCandidatesForInjection.Count == 0)
        {
            GD.PrintErr($"Couldn't find any candidate for property {dependant.Name}.{injectPropertyInfo.Name}");
            if (!Engine.IsEditorHint())
            {
                injectPropertyInfo.SetValue(dependant, null);
            }
            else
            {
                dependant.Set(injectPropertyInfo.Name, default);
            }
        }
        else if (validCandidatesForInjection.Count > 1)
        {
            GD.PrintErr($"Multiple candidate for property {dependant.Name}.{injectPropertyInfo.Name}");
        }
        else
        {
            Node injected = validCandidatesForInjection[0];
            if (!Engine.IsEditorHint())
            {
                injectPropertyInfo.SetValue(dependant, injected);
            }
            else
            {
                dependant.Set(injectPropertyInfo.Name, injected);
            }
            // GD.Print($"Injected {injected.Name} into property {dependant.Name}.{injectPropertyInfo.Name}");
        }
    }

    private static void TryInjectField(Node dependant, FieldInfo injectFieldInfo, Array<Node> siblings)
    {
        dependant.AddToGroup(DependantNodesGroupName);
        Type injectFieldType = injectFieldInfo.FieldType;
        List<Node> validCandidatesForInjection = new();
        foreach (Node sibling in siblings)
        {
            Type siblingType = sibling.GetType();
            if (Engine.IsEditorHint())
            {
                siblingType = (sibling.GetScript().Obj as Script)?.GetScriptType();
            }
            if(siblingType == null) continue;
            
            if (siblingType.IsAssignableTo(injectFieldType))
            {
                validCandidatesForInjection.Add(sibling);
            }
        }
            
        if (validCandidatesForInjection.Count == 0)
        {
            GD.PrintErr($"Couldn't find any candidate for field {dependant.Name}.{injectFieldInfo.Name}");
            if(!Engine.IsEditorHint()){
                injectFieldInfo.SetValue(dependant, null);
            }
            else
            {
                dependant.Set(injectFieldInfo.Name, default);
            }
        }
        else if (validCandidatesForInjection.Count > 1)
        {
            GD.PrintErr($"Multiple candidate for field {dependant.Name}.{injectFieldInfo.Name}");
        }
        else
        {
            Node injected = validCandidatesForInjection[0];
            if(!Engine.IsEditorHint())
            {
                injectFieldInfo.SetValue(dependant, injected);
            }
            else
            {
                dependant.Set(injectFieldInfo.Name, injected);
            }
            // GD.Print($"Injected {injected.Name} into field {dependant.Name}.{injectFieldInfo.Name}");
        }
    }

    private static IEnumerable<PropertyInfo> GetInjectPropertyInfos(Type dependantType)
    {
        return dependantType
            .GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
            .Where(f => f.IsDefined(typeof(InjectAttribute), true));
    }

    private static IEnumerable<FieldInfo> GetInjectFieldInfos(Type dependantType)
    {
        return dependantType
            .GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
            .Where(f => f.IsDefined(typeof(InjectAttribute), true));
    }
    
    private static IEnumerable<PropertyInfo> GetInjectParentPropertyInfos(Type dependantType)
    {
        return dependantType
            .GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
            .Where(f => f.IsDefined(typeof(InjectParentAttribute), true));
    }
    
    private IEnumerable<FieldInfo> GetInjectParentFieldInfo(Type dependantType)
    {
        return dependantType
            .GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
            .Where(f => f.IsDefined(typeof(InjectParentAttribute), true));
    }

    private void UpdateParentInGroup(Node parent)
    {
        
        bool shouldBeInGroup = parent.GetChildCount() > 0 && parent.GetChildren().Any(c => c.IsInGroup(DependantNodesGroupName));
        if (shouldBeInGroup)
        {
            if (!parent.IsInGroup(ParentOfDependantsGroupName))
            {
                parent.AddToGroup(ParentOfDependantsGroupName);
                parent.ChildOrderChanged += ParentOnChildOrderChanged;
            }
        }
        else
        {
            if (parent.IsInGroup(ParentOfDependantsGroupName))
            {
                parent.RemoveFromGroup(ParentOfDependantsGroupName);
                parent.ChildOrderChanged -= ParentOnChildOrderChanged;
            }
        }
    }

    private void ParentOnChildOrderChanged()
    {
        foreach (Node parent in this.GetTree().GetNodesInGroup(ParentOfDependantsGroupName))
        {
            if (parent.GetChildCount() <= 0)
            {
                UpdateParentInGroup(parent);
                continue;
            }
            
            foreach (Node child in parent.GetChildren())
            {
                if (child.IsInGroup(DependantNodesGroupName))
                {
                    UpdateDependencies(child);
                }
            }    
        }
    }

    private void OnNodeAdded(Node node)
    {
        if(!this.IsHandled(node)) return;
        
        this.UpdateDependencies(node);
    }

    private bool IsHandled(Node node)
    {
        return node != this && (!Engine.IsEditorHint() || node.IsPartOfEditedScene());
        // TODO : Check whether it has at least one inject field or property
    }
}
