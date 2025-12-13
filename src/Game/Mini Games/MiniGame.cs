using Game.Players;
using Godot;
using Godot.Collections;

namespace Game.Mini_Games;

[GlobalClass]
public abstract partial class MiniGame : Node
{
    protected PlayerData[] Players;
    
    [Signal]
    public delegate void MiniGameEndedEventHandler(Dictionary<PlayerData,PlayerMiniGameResult> results);

    public virtual void SetPlayerData(PlayerData[] players)
    {
        this.Players = players;
        this.Initialize(players);
    }
    
    protected abstract void Initialize(PlayerData[] players);
    
    
}