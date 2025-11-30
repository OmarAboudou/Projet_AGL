using Godot;
using Godot.Collections;

namespace Composition.Composition_System;

[Tool]
public partial class CompositionSystem : Node
{
    [Export]
    private Array<CSharpScript> _scriptsToInstantiate = new();
    
    public override void _EnterTree()
    {
        base._EnterTree();
        foreach (CSharpScript script in _scriptsToInstantiate)
        {
            Node instance = script.New().As<Node>();
            this.AddChild(instance);
        }
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        foreach (Node child in this.GetChildren())
        {
            child.QueueFree();
        }
    }
}