using System;
using Common.Log;
using Godot;

namespace Common.Utils;

[Tool, GlobalClass]
public abstract partial class PackedSceneWrapper : Resource
{
    private PackedScene _scene;
    protected abstract Type GenericType { get; }
    static PackedSceneWrapper()
    {
        LogSystem.SetLogEnabled<PackedSceneWrapper>(LogType.WARNING, true);
    }

    public static implicit operator PackedScene(PackedSceneWrapper packedSceneWrapper)
    {
        return packedSceneWrapper.Scene;
    }
    
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
            if (!rootNodeType.IsAssignableTo(GenericType))
            {
                LogSystem.Log<PackedSceneWrapper>(LogType.WARNING, $"Scene {value.ResourceName} root node is of type '{rootNodeType.Name}' and is not assignable to type '{GenericType.Name}'");
                this.NotifyPropertyListChanged();
                return;
            }
            
            this._scene = value;
        }
    }
}