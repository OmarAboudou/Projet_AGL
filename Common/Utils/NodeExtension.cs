using Godot;

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
}