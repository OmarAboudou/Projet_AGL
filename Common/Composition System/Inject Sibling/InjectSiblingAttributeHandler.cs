using System;
using System.Collections.Generic;
using System.Linq;
using Composition.Composition_System.Inject;
using Godot;

namespace Composition.Composition_System.Inject_Sibling;

/**
 * Handles Inject Sibling Attributes
 */
[Tool]
public partial class InjectSiblingAttributeHandler : InjectAttributeHandler
{
    protected override Type AttributeType => typeof(InjectSiblingAttribute); 

    protected override List<Node> GetInjectionCandidates(Node injected)
    {
        return injected.GetParent().GetChildren().Where(c => c != injected).ToList();
    }
}