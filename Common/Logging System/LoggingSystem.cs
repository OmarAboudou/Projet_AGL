using System;
using System.Collections.Generic;
using Godot;

namespace Common.Logging_System;

public static class LoggingSystem
{
    private static readonly Dictionary<LogType, HashSet<object>> LogEnabledObjectDictionary = new();
    private static readonly Dictionary<LogType, HashSet<Type>> LogEnabledTypeDictionary = new();
    private delegate void PrintingFunctionDelegate(params object[] messages);

    static LoggingSystem()
    {
        foreach (LogType value in Enum.GetValues<LogType>())
        {
            LogEnabledTypeDictionary[value] = [];
            LogEnabledObjectDictionary[value] = [];
        }
    }
    
    public static void SetLogEnabled(this object source, LogType logType, bool enabled)
    {
        if(enabled) LogEnabledObjectDictionary[logType].Add(source);
        else LogEnabledObjectDictionary[logType].Remove(source);
    }

    public static void SetLogEnabled<T>(LogType logType, bool enabled) where T : class
    {
        if(enabled) LogEnabledObjectDictionary[logType].Add(typeof(T));
        else LogEnabledObjectDictionary[logType].Remove(typeof(T));
    }

    public static void Log(this object source, LogType logType, params object[] messages)
    {
        if(!source.IsLogEnabled(logType)) return;
        
        PrintingFunctionDelegate printingFunctionDelegate = GetLogFunction(logType);
        printingFunctionDelegate?.Invoke(messages: [$"[{Enum.GetName(logType)}]", $"[{source.GetType()}:{source}]", ..messages]);
    }

    public static void Log<T>(LogType logType, params object[] messages) where T : class
    {
        if(!IsLogEnabled<T>(logType)) return;
        
        
        PrintingFunctionDelegate printingFunctionDelegate = GetLogFunction(logType);
        printingFunctionDelegate?.Invoke(messages: [$"[{Enum.GetName(logType)}]", $"[{nameof(T)}]", ..messages]);
    }

    private static PrintingFunctionDelegate GetLogFunction(LogType logType)
    {
        return logType switch
        {
            LogType.INFO => GD.Print,
            LogType.WARNING => GD.PushWarning,
            LogType.ERROR => GD.PrintErr,
            LogType.CRITICAL => GD.PushError,
            _ => messages =>
            {
                GD.PushWarning($"Log type : {logType} is not yet handled. By default {nameof(GD.Print)} is used for printing.");
                GD.Print(messages);
            }
        };
    }
    
    private static bool IsLogEnabled(this object source, LogType logType)
    {
        return LogEnabledObjectDictionary[logType].Contains(source) || LogEnabledTypeDictionary[logType].Contains(source.GetType());
    }

    private static bool IsLogEnabled<T>(LogType logType) where T : class
    {
        return LogEnabledTypeDictionary[logType].Contains(typeof(T));
    }
    
    
    
    
    
}