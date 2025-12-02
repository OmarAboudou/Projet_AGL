using Godot;

namespace Common.Utils;
public abstract partial class TypedPackedScene<T> : PackedScene 
    where T : Node, new()
{
    public T Instantiate()
    {
        return base.Instantiate<T>();
    }

    public T1 Instantiate<T1>() where T1 : T
    {
        return base.Instantiate<T1>();
    }

    public T1 InstantiateOrNull<T1>() where T1 : T
    {
        return base.InstantiateOrNull<T1>();
    }
}