using System;
using Godot;

namespace Game.Players;

public partial class PlayerData(int peerId = 0, String playerName = "", Color playerColor = default) : RefCounted
{
    public int PeerId = peerId;
    public string PlayerName = playerName;
    public Color PlayerColor = playerColor;
}