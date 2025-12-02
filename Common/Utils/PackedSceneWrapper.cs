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
    private readonly Type _expectedSceneType;

    public PackedSceneWrapper(Type expectedSceneType)
    {
        this._expectedSceneType = expectedSceneType;
    }

    public static implicit operator PackedScene(PackedSceneWrapper wrapper) { return wrapper.Scene; }
    
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
            if (!rootNodeType.IsAssignableTo(this._expectedSceneType))
            {
                LogSystem.Log<PackedSceneWrapper>(LogType.WARNING, $"Scene {value.ResourceName} root node is of type '{rootNodeType.Name}' and is not assignable to type '{this._expectedSceneType.Name}'");
                this.NotifyPropertyListChanged();
                return;
            }
            
            this._scene = value;
        }
    }

    public override Variant _Get(StringName property)
    {
        return property.ToString() == nameof(this._expectedSceneType) ? 
            this._expectedSceneType.Name :
            base._Get(property);
    }

    public override Array<Dictionary> _GetPropertyList()
    {
        return
        [
            new Dictionary
            {
                ["name"] = nameof(this._expectedSceneType),
                ["type"] = (int)Variant.Type.String,
                ["usage"] = (int)(PropertyUsageFlags.Editor | PropertyUsageFlags.ReadOnly),
            }
        ];
    }
    
}