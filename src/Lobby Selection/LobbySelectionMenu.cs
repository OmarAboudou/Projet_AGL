using Godot;
using Godot.Collections;

namespace Lobby_Selection;

public partial class LobbySelectionMenu : Control
{
    [Export] private Array<LobbyType> _lobbyTypes = new();
    
    public override void _Ready()
    {
        base._Ready();
    }
}