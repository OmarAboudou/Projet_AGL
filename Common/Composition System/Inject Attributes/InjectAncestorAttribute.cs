using System.Collections.Generic;
using System.Collections.Immutable;
using Godot;

namespace Common.Composition_System.Inject_Attributes;

public class InjectAncestorAttribute : InjectAttribute
{
    public override List<Node> ProcessAttributes(Node injected, ref int injectAttributeIndex, ImmutableArray<InjectAttribute> injectAttributes)
    {
        List<Node> ancestors = new();
        Node current = injected.GetParent();
        while (current != null)
        {
            ancestors.Add(current);
            current = current.GetParent();
        }
        
        return ancestors;
    }
    
}