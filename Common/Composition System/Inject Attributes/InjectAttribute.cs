using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Godot;

namespace Common.Composition_System.Inject_Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public abstract class InjectAttribute : Attribute
{

    public abstract List<Node> ProcessAttributes
    (
        Node injected,
        ref int injectAttributeIndex,
        ImmutableArray<InjectAttribute> injectAttributes
    );

}