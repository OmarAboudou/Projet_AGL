using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using Godot;

namespace Common.Composition_System.Inject_Attributes;

public class InjectAncestorAttribute : InjectAttribute
{
    private List<Node> GetInjectionCandidates(Node injected)
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

    public override List<Node> ProcessAttributes(Node injected, FieldInfo injectedFieldInfo, ref int injectAttributeIndex,
        ImmutableArray<InjectAttribute> injectAttributes)
    {
        return GetInjectionCandidates(injected); 
    }

    public override List<Node> ProcessAttributes(Node injected, PropertyInfo injectedPropertyInfo, ref int injectAttributeIndex,
        ImmutableArray<InjectAttribute> injectAttributes)
    {
        return GetInjectionCandidates(injected); 
    }
}