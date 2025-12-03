using Common.Injection.Inject_Attributes;
using Godot;
using Godot.Collections;

namespace Menu_System.Lobby;

[GlobalClass]
public partial class LobbyTypeSelectionPanel : MenuPanel
{
    [Export, InjectDescendant] private Array<LobbyTypeSelectionButton> _lobbyTypeSelectionButtons = new();
}