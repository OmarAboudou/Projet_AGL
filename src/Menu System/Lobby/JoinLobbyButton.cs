using Godot;

namespace Menu_System.Lobby;

[GlobalClass]
public partial class JoinLobbyButton : LobbyTypeSelectionButton
{
    public override void _Ready()
    {
        base._Ready();
    }

    protected override void OnPressed()
    {
        // TODO : Implement joining lobby
    }
}