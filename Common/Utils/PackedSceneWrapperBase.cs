using System;
using Common.Log;
using Godot;

namespace Common.Utils;

public abstract partial class PackedSceneWrapperBase : Resource
{
    protected PackedScene _scene;
    public abstract Type GenericType { get; }
    
    static PackedSceneWrapperBase()
    {
        LogSystem.SetLogEnabled<PackedSceneWrapperBase>(LogType.WARNING, true);
    }

    public static implicit operator PackedScene(PackedSceneWrapperBase packedSceneWrapperBase)
    {
        return packedSceneWrapperBase.Scene;
    }

    public TWrapper Cast<TWrapper>()
        where TWrapper : PackedSceneWrapperBase, new()
    {
        Type sceneType = Scene.GetRootNodeType();
        if (sceneType == null) throw new NullReferenceException();

        if (sceneType.IsAssignableTo(GenericType))
        {
            return new TWrapper() { Scene = this.Scene };
        }
        throw new InvalidCastException($"Cannot cast scene from type {sceneType.Name} to {GenericType.Name}");
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
                LogSystem.Log<PackedSceneWrapperBase>(LogType.WARNING, $"Scene {value.ResourceName} root node is of type '{rootNodeType.Name}' and is not assignable to type '{GenericType.Name}'");
                this.NotifyPropertyListChanged();
                return;
            }
            
            this._scene = value;
        }
    }
}