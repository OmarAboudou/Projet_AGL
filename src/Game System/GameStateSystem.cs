using System.Collections.Generic;
using System.Linq;
using Common.Composition_System.Inject_Attributes;
using Godot;
using Godot.Collections;

namespace Game_System;

public partial class GameStateSystem : Node
{
    [Export, InjectChild] private Array<GameState> _states = new(); 
    private GameState _currentState;
    
    public List<GameState> GetStates()
    {
        return this._states.ToList();    
    }
    
    public GameState GetCurrentState()
    {
        return this._currentState;
    }
    
    public void ChangeState<T>() where T : GameState, new()
    {
        if (this._currentState != null)
        {
            this._currentState.ExitState();
            this.EmitSignalStateExited(this._currentState);
        }
        this._currentState = this._states.FirstOrDefault(s => s is T);
        if (this._currentState != null)
        {
            this._currentState.EnterState();
            this.EmitSignalStateEntered(this._currentState);
        }
    }
    
    [Signal]
    public delegate void StateEnteredEventHandler(GameState state);
    
    [Signal]
    public delegate void StateExitedEventHandler(GameState state);
}