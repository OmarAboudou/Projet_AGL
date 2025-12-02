using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Godot;

namespace Common.Injection.Inject_Attributes;

public class InjectParentSiblingAttribute : InjectAttribute
{
    public override List<Node> ProcessAttributes(Node injected, ref int injectAttributeIndex,
        ImmutableArray<InjectAttribute> injectAttributes)
    {
        Node parent = injected.GetParent();
        Node grandParent = parent?.GetParent();
        return grandParent == null ? [] : grandParent.GetChildren().Where(c => c != parent).ToList();
    }
    
}