using Common.Utils;
using Godot;

namespace Game_System;

public abstract partial class GameState : Node
{
    public override void _Ready()
    {
        base._Ready();
        this.SetAllProcessing(false);
    }

    public void EnterState()
    {
        this._EnterState();
        this.SetAllProcessing(true);
        this.EmitSignalStateEntered();
    }

    protected abstract void _EnterState();
    
    public void ExitState()
    {
        this._ExitState();
        this.SetAllProcessing(false);
        this.EmitSignalStateExited();
    }

    protected abstract void _ExitState();

    [Signal]
    public delegate void StateEnteredEventHandler();
    
    [Signal]
    public delegate void StateExitedEventHandler();
}