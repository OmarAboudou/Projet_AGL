# Log System

System that allows any type or object to create
and control the visibility of some logs.

## Log Types

The available types of log are
described by the **LogType** enum.

## Enabling/Disabling Log

There are 2 ways to enable/disable logs.
1 way is, to enable it one an instance, 
and the other way is, to enable it on a type.

By default, logs are disabled.

### How to enable or disable logs

```csharp
// Enables logs of type LogType.INFO 
// on that specific instance
this.SetLogEnabled(LogType.INFO, true);

// Disables logs of type LogType.ERROR
// on the type Node
SetLogEnabled<Node>(LogType.ERROR, false);
```

**enabling logs on a type doesn't propagate to subtypes of that type**

## Log

When a certain log type is enabled on
a type or an instance, log messages for
that type or instance can be created as such

```csharp
// Create a log of type LogType.CRITICAL
// for that instance
this.Log(LogType.CRITICAL, obj1, obj2/*,...*/);

// Create a log of type LogType.WARNING
// for the type Node
Log<Node>(LogType.WARNING, obj1, obj2/*,...*/);
```