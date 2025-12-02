using System;
using System.Collections.Generic;
using Common.Log_System;
using Common.Utils;
using Godot;

namespace Main_Menu;

public partial class MainMenuSystem : Control
{
    [Export] private PackedScene _initialScene;
    
    private Stack<PackedScene> _sceneStack = new();
    private Stack<Control> _sceneInstancesStack = new();

    public override void _Ready()
    {
        base._Ready();
        this.SetLogEnabled(LogType.ERROR, true);
        this.StackMenuScene(this._initialScene);
    }

    public void StackMenuScene(PackedScene menuScene)
    {
        Type sceneRootType = menuScene.GetRootNodeType();
        if (!sceneRootType.IsAssignableTo(typeof(Control)))
        {
            this.Log(LogType.ERROR, $"The scene {menuScene.ResourceName} must have a control node at its root.");
            return;
        }
        
        Control instance = menuScene.Instantiate<Control>();
        this.AddChild(instance);
        this._sceneStack.Push(menuScene);
        this._sceneInstancesStack.Push(instance);
    }

    public bool CanGoToPrevious()
    {
        return this._sceneInstancesStack.Count > 1;
    }
    
    public void GoToPrevious()
    {
        if(!this.CanGoToPrevious()) return;

        Control instance = this._sceneInstancesStack.Pop();
        instance.QueueFree();
        this._sceneStack.Pop();
    }
    
}