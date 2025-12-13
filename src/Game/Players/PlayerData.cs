using System;
using Godot;

namespace Game.Players;

[GlobalClass]
public partial class PlayerData : Resource
{
    [Export] public int PeerId;
    [Export] public string PlayerName;
    [Export] public Color PlayerColor = Colors.Black;
    private readonly int _peerId;
    private readonly String _playerName;
    private readonly Color _playerColor;

    public PlayerData() : this(0,"",default)
    {
        
    }
    
    public PlayerData(int peerId = 0, String playerName = "", Color playerColor = default)
    {
        this._peerId = peerId;
        this._playerName = playerName;
        this._playerColor = playerColor;
        this.PeerId = peerId;
        this.PlayerName = playerName;
        this.PlayerColor = playerColor;
    }
}