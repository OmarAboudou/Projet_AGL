using System;
using Common.Log;
using Godot;

namespace Common.Utils;

[Tool]
public partial class PackedSceneWrapper<T> : Resource
where T : class
{
    private PackedScene _scene;

    static PackedSceneWrapper()
    {
        LogSystem.SetLogEnabled<PackedSceneWrapper<T>>(LogType.WARNING, true);
    }
    
    public static implicit operator PackedScene(PackedSceneWrapper<T> typedSceneWrapper)
    {
        return typedSceneWrapper.Scene;
    }

    public T1 Cast<T1, T2>() 
    where T1 : PackedSceneWrapper<T2>, new()
    where T2 : class
    {
        Type sceneType = Scene.GetRootNodeType();
        if (sceneType.IsAssignableTo(typeof(T1)))
        {
            T1 packedSceneWrapper = new T1();
            packedSceneWrapper.Scene = this._scene;
            return packedSceneWrapper;
        }
        throw new InvalidCastException($"Cannot cast scene from type {sceneType.Name} to {typeof(T1).Name}");
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
            if (!rootNodeType.IsAssignableTo(typeof(T)))
            {
                LogSystem.Log<PackedSceneWrapper<T>>(LogType.WARNING, $"Scene {value.ResourceName} root node is of type '{rootNodeType.Name}' and is not assignable to type '{typeof(T).Name}'");
                this.NotifyPropertyListChanged();
                return;
            }
            
            this._scene = value;
        }
    }

    

    public T Instantiate(PackedScene.GenEditState editState = 0)
    {
        return this.Scene.Instantiate<T>(editState);
    }

    public T1 Instantiate<T1>(PackedScene.GenEditState editState = 0) where T1 : class
    {
        return this.Scene.Instantiate<T1>(editState);
    }

    public T1 InstantiateOrNull<T1>(PackedScene.GenEditState editState = 0) where T1 : class
    {
        return this.Scene.InstantiateOrNull<T1>(editState);
    }
}