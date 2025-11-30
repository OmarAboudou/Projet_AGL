using System;
using System.Collections.Generic;
using Composition.Composition_System.Inject;
using Godot;

namespace Composition.Composition_System.Inject_Ancestor;

[Tool]
public partial class InjectAncestorAttributeHandler : InjectAttributeHandler
{
    protected override Type AttributeType => typeof(InjectAncestorAttribute);
    protected override List<Node> GetInjectionCandidates(Node injected)
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