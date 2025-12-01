# Logging System

A logging system that centralize logging logic and
allow to enable/disable different log types
across every instance of that type.

## ILoggable

The ILoggable interface must be implemented 
in order to unlock logging on a type.

When implementing the ILoggable interface, 
the type of the generic type *TSelf* should be
the same type as the type implementing the interface.

### Exemple

```csharp
public class LoggableClass : ILoggable<LoggableClass>
{
    //...
}
```

## Log Types

There a multiple types of logging message for our system:
- Info
- Warning
- Error
- Critical

## Logging Methods

When the ILoggable interface is implemented by a type,
that type unlock new logging methods that 
can be invoked like this :

```csharp
// Replace XXX with a Log Type
LogXXX(obj1, obj2, )
```

## Enabling/Disabling Logs

When the ILoggable interface is implemented by a type,
that type need to implement new static attributes 
to determine which types of logs are enabled.

Here are some exemples :

```csharp
// Replace XXX with a Log Type
public static bool IsLogXXXEnabled => true
    
// Or
// This way allows to change it dynamically at runtime.
public static bool IsLogXXXEnabled {get; private set;} = true
```


