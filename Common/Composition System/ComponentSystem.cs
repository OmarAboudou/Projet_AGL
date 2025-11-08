using Godot;
using Godot.Collections;

namespace Composition.Composition_System;

public partial class ComponentSystem : Node
{
    public override void _EnterTree()
    {
        base._EnterTree();
        this.GetTree().NodeAdded += ComponentAdded;
        this.GetTree().NodeRemoved += ComponentRemoved;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        this.GetTree().NodeAdded -= ComponentAdded;
        this.GetTree().NodeRemoved -= ComponentRemoved;

    }

    private static void ComponentAdded(Node node)
    {
        Node parent = node.GetParent();
        if(parent == null) return;
        
        Array<Node> components = parent.GetChildren();
        foreach (Node component in components)
        {
            if (component is IDependant dependant)
            {
                dependant.DependencyAvailable(node);
            }
        }
    }

    private static void ComponentRemoved(Node node)
    {
        Node parent = node.GetParent();
        if(parent == null) return;

        Array<Node> components = parent.GetChildren();
        foreach (Node component in components)
        {
            if (component is IDependant dependant)
            {
                dependant.DependencyUnavailable(node);
            }
        }
    }

}