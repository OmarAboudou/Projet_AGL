using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Composition.Composition_System.Inject_Attributes;

public class InjectAncestorAttribute : InjectAttribute
{
    private bool _closestFirst = true;

    public InjectAncestorAttribute()
    {
        
    }

    public InjectAncestorAttribute(bool closestFirst)
    {
        this._closestFirst = closestFirst;
    }

    protected override List<Node> GetInjectionCandidates(Node injected)
    {
        List<Node> ancestors = new();
        Node current = injected.GetParent();
        while (current != null)
        {
            ancestors.Add(current);
            current = current.GetParent();
        }

        if (!_closestFirst) ancestors.Reverse();
        
        return ancestors;
    }
}