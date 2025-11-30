using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Common.Composition_System.Inject_Attributes;

public class InjectChildAttribute : InjectAttribute
{
    protected override List<Node> GetInjectionCandidates(Node injected)
    {
        return injected.GetChildren().ToList();
    }
}