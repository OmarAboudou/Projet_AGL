using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Common.Composition_System.Inject_Attributes;

public class InjectSiblingAttribute : InjectAttribute
{
    protected override List<Node> GetInjectionCandidates(Node injected)
    {
        Node parent = injected.GetParent();
        return parent == null ? [] : parent.GetChildren().Where(child => child !=  injected).ToList();
    }
}