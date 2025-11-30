using System;

namespace Composition.Composition_System.Inject;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public abstract class InjectAttribute : Attribute
{
    
}