using Godot;
using System;
using Common.Log;
using Common.Utils;

public partial class App : Node
{
    [Export] private PackedScene _initialScene;
    private Node _currentSceneRootNode;

    private static App This;

    static App()
    {
        LogSystem.SetLogEnabled<App>(LogType.ERROR, true);
    }
    
    public override void _Ready()
    {
        base._Ready();
        if (This != null)
        {
            LogSystem.Log<App>(LogType.ERROR, $"Multiple instances of App are being created. Only the first one is kept.");
            this.QueueFree();
            return;
        }
        
        This = this;
        ChangeScene(This._initialScene);
    }

    public static void ChangeScene(PackedScene scene)
    {
        ChangeScene<Node>(scene);
    }
    
    public static void ChangeScene<T>(PackedScene scene) where T : class
    {
        Type sceneRootType = scene.GetRootNodeType();
        if (!sceneRootType.IsAssignableTo(typeof(T)))
        {
            throw new ArgumentException($"Scene {scene.ResourceName} has a root node of type '{sceneRootType.Name}' which is not assignable to type '{typeof(T).Name}'");
        }

        T sceneInstance = scene.Instantiate<T>();
        
        if (This._currentSceneRootNode != null)
        {
            This._currentSceneRootNode.QueueFree();
        }
        
        This._currentSceneRootNode = sceneInstance as Node;
        This.AddChild(This._currentSceneRootNode);
    }

}
