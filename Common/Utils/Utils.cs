using System;
using Godot;

namespace Common.Utils;

public static class Utils
{
    public static Type GetRootNodeType(this PackedScene scene)
    {
        SceneState state = scene.GetState();
        const int rootIndex = 0;
        StringName rootNodeGodotType = state.GetNodeType(0);
        CSharpScript rootNodeScript = null;
        int propCount = state.GetNodePropertyCount(0);
        
        for (int propIdx = 0; propIdx < propCount; propIdx++)
        {
            StringName propName = state.GetNodePropertyName(rootIndex, propIdx);
            if (propName == "script")
            {
                rootNodeScript = state.GetNodePropertyValue(rootIndex, propIdx).As<CSharpScript>();
                break;
            }
        }

        if (rootNodeScript == null)
        {
            return typeof(Node).Assembly.GetType($"Godot.{rootNodeGodotType}");
        }

        return rootNodeScript.GetScriptType();

    }

    public static Type GetScriptType(this Script script) => (script as CSharpScript)?.New().Obj?.GetType();
    
}