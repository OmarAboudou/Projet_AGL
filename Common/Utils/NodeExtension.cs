using System;
using System.Reflection;
using Godot;

namespace Common.Utils;

public static class NodeExtension
{
    public static Type GetNodeType(this Node node)
    {
        Type nodeType = node.GetType();
        if (!Engine.IsEditorHint()) return nodeType;
        
        Type scriptType = (node.GetScript().Obj as Script)?.GetScriptType();
        if(scriptType != null) return scriptType;
        
        return nodeType;
    }
    
    public static void SetField(this Node node, Variant value, FieldInfo fieldInfo)
    {
        if (!Engine.IsEditorHint())
        {
            fieldInfo.SetValue(node, value);
        }
        else
        {
            node.Set(fieldInfo.Name, value);
        }
    }

    public static void ClearField(this Node node, FieldInfo fieldInfo)
    {
        if (!Engine.IsEditorHint())
        {
            fieldInfo.SetValue(node, null);
        }
        else
        {
            node.Set(fieldInfo.Name, default);
        }
    }

    public static void SetProperty(this Node node, Variant value, PropertyInfo propertyInfo)
    {
        if (!Engine.IsEditorHint())
        {
            propertyInfo.SetValue(node, value);
        }
        else
        {
            node.Set(propertyInfo.Name, value);
        }
    }

    public static void ClearProperty(this Node node, PropertyInfo propertyInfo)
    {
        if (!Engine.IsEditorHint())
        {
            propertyInfo.SetValue(node, null);
        }
        else
        {
            node.Set(propertyInfo.Name, default);
        }
    }

    public static void SetAllProcessing(this Node node, bool enabled)
    {
        node.SetProcess(enabled);
        node.SetPhysicsProcess(enabled);
        
        node.SetProcessInput(enabled);
        node.SetProcessShortcutInput(enabled);
        node.SetProcessUnhandledInput(enabled);
        node.SetProcessUnhandledKeyInput(enabled);
    }
}