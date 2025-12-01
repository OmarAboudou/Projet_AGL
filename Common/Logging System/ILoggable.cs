namespace Common.Logging_System;

public interface ILoggable<TSelf>
where TSelf : ILoggable<TSelf>
{

    public static abstract bool IsLogInfoEnabled { get; }
    public static abstract bool IsLogWarningEnabled { get; }
    public static abstract bool IsLogErrorEnabled { get; }
    public static abstract bool IsLogCriticalEnabled { get; }
    public static virtual string Separator => LoggingSystem.DefaultSeparator;
}