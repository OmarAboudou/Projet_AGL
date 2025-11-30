using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Godot;

namespace Common.Composition_System.Inject_Attributes;

public class InjectParentSiblingAttribute : InjectAttribute
{
    protected List<Node> GetInjectionCandidates(Node injected)
    {
        Node parent = injected.GetParent();
        Node grandParent = parent?.GetParent();
        return grandParent == null ? [] : grandParent.GetChildren().Where(c => c != parent).ToList();
    }

    public override List<Node> ProcessAttributes(Node injected, FieldInfo injectedFieldInfo, ref int injectAttributeIndex,
        ImmutableArray<InjectAttribute> injectAttributes)
    {
        return GetInjectionCandidates(injected);
    }

    public override List<Node> ProcessAttributes(Node injected, PropertyInfo injectedPropertyInfo, ref int injectAttributeIndex,
        ImmutableArray<InjectAttribute> injectAttributes)
    {
        return GetInjectionCandidates(injected);
    }
}