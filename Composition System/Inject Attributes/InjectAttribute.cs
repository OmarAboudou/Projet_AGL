using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using common_project.Utils;
using Godot;

namespace common_project.Composition_System.Inject_Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public abstract class InjectAttribute : Attribute
{
    public void Inject(Node injected, FieldInfo injectedFieldInfo)
    {
        List<Node> candidates =
            injectedFieldInfo
                .GetCustomAttribute<InjectAttribute>()
                ?.GetInjectionCandidates(injected);
        if(candidates == null) return;
            
        List<Node> validCandidates = 
            candidates
                .Where(c => c.GetNodeType().IsAssignableTo(injectedFieldInfo.FieldType))
                .ToList();

        if (validCandidates.Count <= 0)
        {
            GD.PrintErr($"Couldn't find any candidate for field {injected.Name}.{injectedFieldInfo.Name}");
            injected.ClearField(injectedFieldInfo);
            return;
        }
        
        if (validCandidates.Count > 1)
        {
            GD.PushWarning($"Multiple candidates for field {injected.Name}.{injectedFieldInfo.Name}");
        }
        Node injection = validCandidates[0];
        injected.SetField(injection, injectedFieldInfo);
        GD.Print($"Injected {injection.Name} into field {injected.Name}.{injectedFieldInfo.Name}");
    }

    public void Inject(Node injected, PropertyInfo injectedPropertyInfo)
    {
        List<Node> candidates = 
            injectedPropertyInfo
                .GetCustomAttribute<InjectAttribute>()
                ?.GetInjectionCandidates(injected);
        if(candidates == null) return;
            
        List<Node> validCandidates = 
            candidates
                .Where(c => c.GetNodeType().IsAssignableTo(injectedPropertyInfo.PropertyType))
                .ToList();

        if (validCandidates.Count <= 0)
        {
            GD.PrintErr($"Couldn't find any candidate for property {injected.Name}.{injectedPropertyInfo.Name}");
            injected.ClearProperty(injectedPropertyInfo);
            return;
        }
        
        if (validCandidates.Count > 1)
        {
            GD.PushWarning($"Multiple candidates for property {injected.Name}.{injectedPropertyInfo.Name}");
        }
        Node injection = validCandidates[0];
        injected.SetProperty(injection, injectedPropertyInfo);
        GD.Print($"Injected {injection.Name} into property {injected.Name}.{injectedPropertyInfo.Name}");
    }

    protected abstract List<Node> GetInjectionCandidates(Node injected);
}