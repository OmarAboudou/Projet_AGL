using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Composition.Composition_System.Inject_Attributes;
using Godot;

namespace Composition.Composition_System;

[Tool]
public partial class InjectAttributeHandler<TAttribute> : Node
where TAttribute : InjectAttribute, new()
{
    private readonly List<Node> _handledNodes = new();
    
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
    
    private void UpdateInjection(Node injected)
    {
        Type nodeType = injected.GetNodeType();

        this.UpdateFieldsInjection(injected, nodeType);
        this.UpdatePropertiesInjection(injected, nodeType);
    }
    
    private void UpdateFieldsInjection(Node injected, Type nodeType)
    {
        List<FieldInfo> injectedFieldInfos = this.GetInjectedFields(nodeType);
        foreach (FieldInfo injectedFieldInfo in injectedFieldInfos)
        {
            InjectAttribute attribute = injectedFieldInfo.GetCustomAttribute<InjectAttribute>();

            if (attribute != null) attribute.Inject(injected, injectedFieldInfo);
        }
    }

    private void UpdatePropertiesInjection(Node injected, Type nodeType)
    {
        List<PropertyInfo> injectedPropertyInfos = this.GetInjectedProperties(nodeType);
        foreach (PropertyInfo injectedPropertyInfo in injectedPropertyInfos)
        {
            InjectAttribute attribute = injectedPropertyInfo.GetCustomAttribute<InjectAttribute>();

            if (attribute != null) attribute.Inject(injected, injectedPropertyInfo);
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
        Type nodeType = node.GetNodeType();

        return this.HasAnyInjectedFields(nodeType) || this.HasAnyInjectedProperties(nodeType);
    }

    private List<FieldInfo> GetInjectedFields(Type type)
    {
        return type
            .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            .Where
            (
                info => info
                    .GetCustomAttributes<TAttribute>()
                    .Any()
            )
            .ToList();
    }

    private List<PropertyInfo> GetInjectedProperties(Type type)
    {
        return type
            .GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            .Where
            (
                info => info
                .GetCustomAttributes<TAttribute>()
                .Any()
            )
            .ToList();
    }
    
    private bool HasAnyInjectedFields(Type type)
    {
        return this.GetInjectedFields(type).Any();
    }

    private bool HasAnyInjectedProperties(Type type)
    {
        return this.GetInjectedProperties(type).Any();
    }
    
}