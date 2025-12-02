using System;
using Common.Log;
using Godot;
using Godot.Collections;

namespace Common.Utils;

[Tool, GlobalClass]
public partial class PackedSceneWrapper : Resource
{
    public PackedSceneWrapper() { }

    private PackedScene _scene;
    private Type ExpectedSceneType { get; } = typeof(object);

    static PackedSceneWrapper()
    {
        LogSystem.SetLogEnabled<PackedSceneWrapper>(LogType.WARNING, true);
        LogSystem.SetLogEnabled<PackedSceneWrapper>(LogType.ERROR, true);
        LogSystem.SetLogEnabled<PackedSceneWrapper>(LogType.CRITICAL, true);
    }
    
    public PackedSceneWrapper(Type expectedSceneType)
    {
        this.ExpectedSceneType = expectedSceneType;
    }

    public Node Instantiate(PackedScene.GenEditState editState = PackedScene.GenEditState.Disabled)
    {
        return this.Scene.Instantiate(editState);
    }

    public T Instantiate<T>(PackedScene.GenEditState editState = PackedScene.GenEditState.Disabled) where T : class
    {
        return this.Scene.Instantiate<T>(editState);
    }

    public T InstantiateOrNull<T>(PackedScene.GenEditState editState = PackedScene.GenEditState.Disabled) where T : class
    {
        return this.Scene.InstantiateOrNull<T>(editState);
    }

    public bool CanInstantiate()
    {
        return this.Scene.CanInstantiate();
    }

    public Error Pack(Node path)
    {
        return this.Scene.Pack(path);
    }

    public static implicit operator PackedScene(PackedSceneWrapper wrapper) { return wrapper.Scene; }
    
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
            if (!rootNodeType.IsAssignableTo(this.ExpectedSceneType))
            {
                LogSystem.Log<PackedSceneWrapper>(LogType.WARNING, $"Scene {value.ResourceName} root node is of type '{rootNodeType.Name}' and is not assignable to type '{this.ExpectedSceneType?.Name}'");
                this.NotifyPropertyListChanged();
                return;
            }
            
            this._scene = value;
        }
    }

    public override Variant _Get(StringName property)
    {
        return property.ToString() == nameof(this.ExpectedSceneType) ? 
            this.ExpectedSceneType.Name :
            base._Get(property);
    }

    public override Array<Dictionary> _GetPropertyList()
    {
        return
        [
            new Dictionary
            {
                ["name"] = nameof(this.ExpectedSceneType),
                ["type"] = (int)Variant.Type.String,
                ["usage"] = (int)(PropertyUsageFlags.Editor | PropertyUsageFlags.ReadOnly),
            }
        ];
    }
    
}