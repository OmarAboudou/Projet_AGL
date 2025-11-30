using System;
using System.Linq;
using Composition.Composition_System.Inject_Attributes;
using Godot;

namespace Composition.Composition_System;

[Tool]
public partial class CompositionSystem : Node
{
    
    public override void _EnterTree()
    {
        base._EnterTree();
        Type[] injectAttributeTypes = 
            typeof(InjectAttribute)
                .Assembly
                .GetTypes()
                .Where(t => t.IsAssignableTo(typeof(InjectAttribute)) && !t.IsAbstract && !t.IsInterface)
                .ToArray();
        foreach (Type injectAttributeType in injectAttributeTypes)
        {
            Type injectAttributeHandlerType = typeof(InjectAttributeHandler<>).MakeGenericType(injectAttributeType);
            Node instance = (Node) Activator.CreateInstance(injectAttributeHandlerType);
            this.AddChild(instance);
        }
    }

    public override void _ExitTree()
    {
        base._ExitTree();
    }
}