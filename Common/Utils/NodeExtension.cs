using System.Linq;
using Godot;
using Godot.Collections;

namespace Common.Utils;

public static class NodeExtension
{
    public static void SetAllProcessing(this Node node, bool enabled)
    {
        node.SetProcess(enabled);
        node.SetPhysicsProcess(enabled);
        
        node.SetProcessInput(enabled);
        node.SetProcessShortcutInput(enabled);
        node.SetProcessUnhandledInput(enabled);
        node.SetProcessUnhandledKeyInput(enabled);
    }

    public static T GetAutoloadOfType<T>(this Node node) where T : class
    {
        return node.GetTree().GetAutoloadOfType<T>();
    }
    
    public static T GetAutoloadOfType<T>(this SceneTree sceneTree) where T : class
    {
        Array<Node> children = sceneTree.Root.GetChildren();
        foreach (Node child in children)
        {
            if(child is T tChild) return tChild;
        }
        return null;
    }
    
    
}