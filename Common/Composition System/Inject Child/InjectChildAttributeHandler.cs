using System;
using System.Collections.Generic;
using System.Linq;
using Composition.Composition_System.Inject;
using Godot;

namespace Composition.Composition_System.Inject_Child;

/**
 * Handles Inject Child attributes
 */
[Tool]
public partial class InjectChildAttributeHandler : InjectAttributeHandler
{
    protected override Type AttributeType => typeof(InjectChildAttribute);
    protected override List<Node> GetInjectionCandidates(Node injected)
    {
        return injected.GetChildren().ToList();
    }
}