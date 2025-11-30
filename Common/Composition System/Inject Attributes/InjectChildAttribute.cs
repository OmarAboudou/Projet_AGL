using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Godot;

namespace Common.Composition_System.Inject_Attributes;

public class InjectChildAttribute : InjectAttribute
{
    public override List<Node> ProcessAttributes(
        Node injected, 
        FieldInfo injectedFieldInfo,
        ref int injectAttributeIndex,
        ImmutableArray<InjectAttribute> injectAttributes)
    {
        return injected.GetChildren().ToList();
    }

    public override List<Node> ProcessAttributes(
        Node injected, 
        PropertyInfo injectedPropertyInfo,
        ref int injectAttributeIndex,
        ImmutableArray<InjectAttribute> injectAttributes)
    {
        return injected.GetChildren().ToList();
    }
}