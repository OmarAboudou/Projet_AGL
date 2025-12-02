using System;
using Godot;

namespace Common.Utils;

[Tool]
public abstract partial class PackedSceneWrapper<T> : PackedSceneWrapperBase
where T : class
{
    public sealed override Type GenericType => typeof(T);
    
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