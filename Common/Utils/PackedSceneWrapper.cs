using System;
using Common.Log;
using Godot;

namespace Common.Utils;

public abstract partial class PackedSceneWrapper<T> : Resource
    where T : class
{
    private PackedScene _scene { get; set; }
    
    [Export]
    public PackedScene Scene
    {
        get => this._scene;
        set
        {
            if (value == null)
            {
                this._scene = null;
                return;
            }
            
            Type rootNodeType = value.GetRootNodeType();
            if (!rootNodeType.IsAssignableTo(typeof(T)))
            {
                LogSystem.Log<PackedSceneWrapper<T>>(LogType.WARNING, $"Scene {value.ResourceName} root node is of type '{rootNodeType.Name}' and is not assignable to type '{typeof(T).Name}'");
                this.NotifyPropertyListChanged();
                return;
            }
            
            this._scene = value;
        }
    }
}