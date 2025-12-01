using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Godot;

namespace Common.Logging_System;

public static class LoggingSystem
{
    public static readonly string DefaultSeparator = "\n";

    static LoggingSystem()
    {
        IEnumerable<Type> loggableTypes = 
            typeof(ILoggable<>)
                .Assembly
                .GetTypes()
                .Where(t =>
                {
                    foreach (Type i in t.GetInterfaces())
                    {
                        if (i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ILoggable<>))
                        {
                            return true;
                        }
                    }
                    return false;
                });
        
        foreach (Type type in loggableTypes)
        {

        }
    }
    
    public static void LogInfo<T>(this ILoggable<T> loggable, params object[] messages) where T : ILoggable<T>
    {
        if(!loggable.IsLogInfoEnabled()) return;
        
        GD.Print(CreateMessage("INFO", loggable, messages));
    }

    public static void LogWarning<T>(this ILoggable<T> loggable, params object[] messages) where T : ILoggable<T>
    {
        if(!loggable.IsLogWarningEnabled()) return;

        GD.PushWarning(CreateMessage("WARNING", loggable, messages));
        
    }

    public static void LogError<T>(this ILoggable<T> loggable, params object[] messages) where T : ILoggable<T>
    {
        if(!loggable.IsLogErrorEnabled()) return;
        
        GD.PrintErr(CreateMessage("ERROR", loggable, messages));
    }

    public static void LogCritical<T>(this ILoggable<T> loggable, params object[] messages) where T : ILoggable<T>
    {
        if(!loggable.IsLogCriticalEnabled()) return;
        
        GD.PushError(CreateMessage("CRITICAL", loggable, messages));

    }

    private static string CreateMessage<T>(string prefix, ILoggable<T> loggable, params object[] messages) where T : ILoggable<T>
    {
        return $"[{prefix}] [{typeof(T).Name}:{loggable}] {String.Join(T.Separator, messages)}";
    }

    private static bool IsLogEnabled<T>(this ILoggable<T> loggable, String propertyName) where T : ILoggable<T>
    {
        return 
            (bool)
            typeof(T)
                .GetProperty(propertyName, BindingFlags.Static | BindingFlags.Public)
                .GetValue(null);
        
    }

    private static bool IsLogInfoEnabled<T>(this ILoggable<T> loggable) where T : ILoggable<T>
    {
        return IsLogEnabled(loggable, nameof(ILoggable<T>.IsLogInfoEnabled));
    }

    public static bool IsLogWarningEnabled<T>(this ILoggable<T> loggable) where T : ILoggable<T>
    {
        return IsLogEnabled(loggable, nameof(ILoggable<T>.IsLogWarningEnabled));
    }

    public static bool IsLogErrorEnabled<T>(this ILoggable<T> loggable) where T : ILoggable<T>
    {
        return IsLogEnabled(loggable, nameof(ILoggable<T>.IsLogErrorEnabled));
    }

    public static bool IsLogCriticalEnabled<T>(this ILoggable<T> loggable) where T : ILoggable<T>
    {
        return IsLogEnabled(loggable, nameof(ILoggable<T>.IsLogCriticalEnabled));
    }
    
}