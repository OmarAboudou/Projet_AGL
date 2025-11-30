using System;
using Godot;

namespace Common.Utils;

public static class Utils
{
    public static Type GetRootNodeType(this PackedScene scene)
    {
        string rootNodeType = scene._Bundled["names"].AsGodotArray<string>()[1];
        if (rootNodeType == "script")
        {
            CSharpScript script = scene._Bundled["variants"].AsGodotArray()[1].Obj as CSharpScript;
            return script.New().Obj.GetType();
        }
        else
        {
            return typeof(Node).Assembly.GetType($"Godot.{rootNodeType}");
        }
    }

    public static Type GetScriptType(this Script script) => (script as CSharpScript)?.New().Obj?.GetType();
    
}