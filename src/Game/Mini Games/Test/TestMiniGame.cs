using Game.Players;
using Godot;
using Godot.Collections;

namespace Game.Mini_Games.Test;


public partial class TestMiniGame : MiniGame
{
    [Export] private Timer _timer;
    
    protected override void Initialize(PlayerData[] players)
    {
        
    }

    public override void _Ready()
    {
        base._Ready();
        this._timer.Timeout += this.OnTimeout;
    }

    private void OnTimeout()
    {
        GD.Print("TEST MINI GAME DONE !!!");
        Dictionary<PlayerData,PlayerMiniGameResult> results = new();
        foreach (PlayerData playerData in this.Players)
        {
            results[playerData] = PlayerMiniGameResult.Win;
        }
        this.EmitSignalMiniGameEnded(results);
    }
}