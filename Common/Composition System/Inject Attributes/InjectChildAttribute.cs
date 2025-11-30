using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Godot;

namespace Common.Composition_System.Inject_Attributes;

public class InjectChildAttribute : InjectAttribute
{
    public override List<Node> ProcessAttributes(
        Node injected,
        ref int injectAttributeIndex,
        ImmutableArray<InjectAttribute> injectAttributes)
    {
        return injected.GetChildren().ToList();
    }
    
}