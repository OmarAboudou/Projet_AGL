using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Godot;

namespace Common.Injection.Inject_Attributes;

public class InjectDescendantAttribute : InjectAttribute
{
    public override List<Node> ProcessAttributes(Node injected, ref int injectAttributeIndex, ImmutableArray<InjectAttribute> injectAttributes)
    {
        return this.GetChildrenDepthFirst(injected);
    }

    private List<Node> GetChildrenDepthFirst(Node parent)
    {
        List<Node> result = new List<Node>();
        Stack<Node> stack  = new Stack<Node>();

        // On pousse les enfants directs du parent dans la pile
        // en ordre inversé pour que le premier enfant soit traité en premier.
        for (int i = parent.GetChildCount() - 1; i >= 0; i--)
        {
            stack.Push(parent.GetChild(i));
        }

        while (stack.Count > 0)
        {
            Node current = stack.Pop();
            result.Add(current);

            // Même logique : on pousse les enfants en ordre inversé
            // pour respecter l'ordre naturel dans result.
            for (int i = current.GetChildCount() - 1; i >= 0; i--)
            {
                stack.Push(current.GetChild(i));
            }
        }

        return result;
    }
}