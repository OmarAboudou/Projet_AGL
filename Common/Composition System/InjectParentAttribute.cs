using System;

namespace Composition.Composition_System;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class InjectParentAttribute : Attribute 
{
    
}