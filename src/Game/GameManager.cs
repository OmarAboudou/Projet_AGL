using Game.Mini_Games;
using Game.Players;
using Godot;

namespace Game;

public partial class GameManager : Node
{
    [Export] private PlayerData[] _players;
    [Export] private MiniGameData[] _miniGames;
    
    public void Initialize(PlayerData[] players, MiniGameData[] miniGames)
    {
        this._players = players;
        this._miniGames = miniGames;
    }
}