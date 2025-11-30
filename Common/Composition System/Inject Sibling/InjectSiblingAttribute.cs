using System;
using Composition.Composition_System.Inject;

namespace Composition.Composition_System.Inject_Sibling;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class InjectSiblingAttribute : InjectAttribute
{
    
}