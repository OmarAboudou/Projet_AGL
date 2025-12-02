using Common.Log;
using Godot;

namespace Common.Utils;

public abstract partial class TypeSafePackedSceneHolder<T> : Resource
where T : class
{
    private PackedScene _scene;

    static TypeSafePackedSceneHolder()
    {
        LogSystem.SetLogEnabled<TypeSafePackedSceneHolder<T>>(LogType.WARNING, true);
    }
    
    [Export]
    public PackedScene Scene
    {
        get => this._scene;
        private set
        {
            if(value == null) return;

            if (!value.GetRootNodeType().IsAssignableTo(typeof(T)))
            {
                this.Log(LogType.WARNING, $"Scene {value.ResourceName} must have a root node of a type assignable to type {typeof(T).Name}");
                return;
            }
            
            this._scene = value;
        }
    }
}