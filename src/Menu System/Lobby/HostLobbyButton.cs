using Common.Log;
using Godot;

namespace Menu_System.Lobby;

[GlobalClass]
public partial class HostLobbyButton : LobbyTypeSelectionButton
{
    public override void _Ready()
    {
        base._Ready();
        this.SetLogEnabled(LogType.WARNING, true);
    }

    protected override void OnPressed()
    {
        // TODO : Implement hosting lobby
        this.Log(LogType.WARNING, $"Hosting lobby is not implemented yet.");
    }
}