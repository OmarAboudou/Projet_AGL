using System;
using Godot;

public readonly struct PlayerData(int peerId, String playerName, Color playerColor)
{
    public readonly int PeerId = peerId;
    public readonly string PlayerName = playerName;
    public readonly Color PlayerColor = playerColor;
}