using Godot;

namespace Game_System;

public abstract partial class GameState : Node
{
    public void EnterState()
    {
        this._EnterState();
        this.EmitSignalStateEntered();
    }

    protected abstract void _EnterState();
    
    public void ExitState()
    {
        this._ExitState();
        this.EmitSignalStateExited();
    }

    protected abstract void _ExitState();

    [Signal]
    public delegate void StateEnteredEventHandler();
    
    [Signal]
    public delegate void StateExitedEventHandler();
}