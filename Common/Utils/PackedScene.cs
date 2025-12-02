using Godot;

namespace Common.Utils;
public abstract partial class PackedScene<T> : PackedScene 
    where T : Node, new()
{
    public new T Instantiate(GenEditState editState = 0)
    {
        return base.Instantiate<T>(editState);
    }

    public new T1 Instantiate<T1>(GenEditState editState = 0) where T1 : T
    {
        return base.Instantiate<T1>(editState);
    }

    public new T1 InstantiateOrNull<T1>(GenEditState editState = 0) where T1 : T
    {
        return base.InstantiateOrNull<T1>(editState);
    }
}