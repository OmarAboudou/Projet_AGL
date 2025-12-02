using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Godot;

namespace Common.Injection.Inject_Attributes;

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