using System;
using System.Collections.Generic;
using Composition.Composition_System.Inject;
using Godot;

namespace Composition.Composition_System.Inject_Parent;

/**
 * Handles Inject Parent attributes
 */
[Tool]
public partial class InjectParentAttributeHandler : InjectAttributeHandler
{
    protected override Type AttributeType => typeof(InjectParentAttribute);

    protected override List<Node> GetInjectionCandidates(Node injected)
    {
        Node parent =  injected.GetParentOrNull<Node>();
        if (parent == null) return [];
        return [injected.GetParent()];
    }
}