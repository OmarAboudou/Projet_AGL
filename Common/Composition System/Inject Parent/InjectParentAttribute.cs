using System;
using Composition.Composition_System.Inject;

namespace Composition.Composition_System.Inject_Parent;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class InjectParentAttribute : InjectAttribute 
{
    
}