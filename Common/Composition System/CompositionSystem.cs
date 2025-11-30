using System;
using System.Linq;
using Composition.Composition_System.Inject;
using Godot;

namespace Composition.Composition_System;

[Tool]
public partial class CompositionSystem : Node
{
    
    public override void _EnterTree()
    {
        base._EnterTree();
        Type[] injectAttributeHandlerTypes = 
            typeof(InjectAttributeHandler)
                .Assembly
                .GetTypes()
                .Where(t => t.IsAssignableTo(typeof(InjectAttributeHandler)) && !t.IsAbstract && !t.IsInterface)
                .ToArray();;
        foreach (Type type in injectAttributeHandlerTypes)
        {
            Node instance = (Node)Activator.CreateInstance(type);
            this.AddChild(instance);
        }
    }

    public override void _ExitTree()
    {
        base._ExitTree();
    }
}