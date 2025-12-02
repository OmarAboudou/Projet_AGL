using System;
using Common.Log;
using Godot;

namespace Common.Utils;

public abstract partial class PackedSceneWrapper : Resource
{
    public PackedSceneWrapper() : this(typeof(object)) { }

    private PackedScene _scene;
    private readonly Type _expectedSceneType;

    public PackedSceneWrapper(Type expectedSceneType)
    {
        this._expectedSceneType = expectedSceneType;
    }

    [Export]
    public PackedScene Scene
    {
        get => this._scene;
        private set
        {
            if (value == null)
            {
                this._scene = null;
                return;
            }
            
            Type rootNodeType = value.GetRootNodeType();
            if (!rootNodeType.IsAssignableTo(this._expectedSceneType))
            {
                LogSystem.Log<PackedSceneWrapper>(LogType.WARNING, $"Scene {value.ResourceName} root node is of type '{rootNodeType.Name}' and is not assignable to type '{this._expectedSceneType.Name}'");
                this.NotifyPropertyListChanged();
                return;
            }
            
            this._scene = value;
        }
    }
}