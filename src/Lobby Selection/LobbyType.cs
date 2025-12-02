using Godot;

namespace Lobby_Selection;

[GlobalClass]
public partial class LobbyType : Resource
{
    [Export] public string LobbyTypeName { get; private set; }
    [Export] public PackedScene MenuScene { get; private set; }
}