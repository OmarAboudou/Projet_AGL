using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Godot;

namespace Common.Composition_System.Inject_Attributes;

public class InjectSiblingAttribute : InjectAttribute
{
    public override List<Node> ProcessAttributes(Node injected, FieldInfo injectedFieldInfo, ref int injectAttributeIndex,
        ImmutableArray<InjectAttribute> injectAttributes)
    {
        Node parent = injected.GetParent();
        return parent == null ? [] : parent.GetChildren().Where(child => child !=  injected).ToList();
    }

    public override List<Node> ProcessAttributes(Node injected, PropertyInfo injectedPropertyInfo, ref int injectAttributeIndex,
        ImmutableArray<InjectAttribute> injectAttributes)
    {
        Node parent = injected.GetParent();
        return parent == null ? [] : parent.GetChildren().Where(child => child !=  injected).ToList();
    }
}