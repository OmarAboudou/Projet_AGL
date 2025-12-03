using Godot;
using Godot.Collections;

namespace Menu_System.Lobby;

[GlobalClass]
public partial class LobbyTypeSelectionPanel : MenuPanel
{
    [Export] private Array<LobbyTypeSelectionButton> _lobbyTypeSelectionButtons = new();
}