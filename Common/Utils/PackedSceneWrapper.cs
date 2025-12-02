using System;
using Common.Log;
using Godot;

namespace Common.Utils;

public abstract partial class PackedSceneWrapper(Type expectedSceneType) : Resource
{
    protected PackedSceneWrapper() : this(typeof(object)) { }

    private PackedScene _scene;
    
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
            if (!rootNodeType.IsAssignableTo(expectedSceneType))
            {
                LogSystem.Log<PackedSceneWrapper>(LogType.WARNING, $"Scene {value.ResourceName} root node is of type '{rootNodeType.Name}' and is not assignable to type '{expectedSceneType.Name}'");
                this.NotifyPropertyListChanged();
                return;
            }
            
            this._scene = value;
        }
    }
}