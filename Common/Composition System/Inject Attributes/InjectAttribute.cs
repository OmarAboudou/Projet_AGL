using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Common.Utils;
using Godot;

namespace Common.Composition_System.Inject_Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public abstract class InjectAttribute : Attribute
{

    public abstract List<Node> ProcessAttributes
    (
        Node injected, 
        FieldInfo injectedFieldInfo,
        ref int injectAttributeIndex,
        ImmutableArray<InjectAttribute> injectAttributes
    );

    public abstract List<Node> ProcessAttributes
    (
        Node injected,
        PropertyInfo injectedPropertyInfo,
        ref int injectAttributeIndex,
        ImmutableArray<InjectAttribute> injectAttributes
    );

}