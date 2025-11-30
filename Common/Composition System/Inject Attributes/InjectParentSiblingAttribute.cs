/*using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Common.Composition_System.Inject_Attributes;

public class InjectParentSiblingAttribute : InjectAttribute
{
    protected override List<Node> GetInjectionCandidates(Node injected)
    {
        Node parent = injected.GetParent();
        Node grandParent = parent?.GetParent();
        return grandParent == null ? [] : grandParent.GetChildren().Where(c => c != parent).ToList();
    }
}*/