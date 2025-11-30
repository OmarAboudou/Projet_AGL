using System.Collections.Generic;
using Godot;

namespace common_project.Composition_System.Inject_Attributes;

public class InjectParentAttribute : InjectAttribute 
{
    protected override List<Node> GetInjectionCandidates(Node injected)
    {
        Node parent = injected.GetParent();
        return parent == null ? [] : [parent];
    }
}