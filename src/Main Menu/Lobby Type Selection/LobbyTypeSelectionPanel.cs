using Godot;
using Godot.Collections;
using Main_Menu.Lobby_Type_Selection.Lobby_Type_Button;

namespace Main_Menu.Lobby_Type_Selection;

[GlobalClass]
public partial class LobbyTypeSelectionPanel : MenuPanel
{
    [Export] private Array<LobbyTypeButton> _lobbyTypeButtons = new();

    public override void _Ready()
    {
        base._Ready();
        foreach (LobbyTypeButton button in _lobbyTypeButtons)
        {
            button.RequestNewPanel += this.EmitSignalRequestNewPanel;
        }
    }
}