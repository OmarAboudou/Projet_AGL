using Common.Log;
using Godot;

namespace Menu_System.Lobby;

[GlobalClass]
public partial class JoinLobbyButton : LobbyTypeSelectionButton
{
    public override void _Ready()
    {
        base._Ready();
        this.SetLogEnabled(LogType.WARNING, true);
    }

    protected override void OnPressed()
    {
        // TODO : Implement joining lobby
        this.Log(LogType.WARNING, $"Joining lobby is not implemented yet.");
    }
}