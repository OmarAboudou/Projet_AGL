using Common.Log;
using Godot;

namespace Common.Utils;

[Tool]
public abstract partial class PackedSceneWrapper<T> : Resource
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
            
            if (!value.GetRootNodeType().IsAssignableTo(typeof(T)))
            {
                LogSystem.Log<PackedSceneWrapper<T>>(LogType.WARNING, $"Scene {value.ResourceName} must have a root node of a type assignable to type {typeof(T).Name}");
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