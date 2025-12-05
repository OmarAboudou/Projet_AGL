using System;
using Godot;
using Server;

namespace Main_Menu.Lobby_Type_Selection.Lobby_Type_Button;

[GlobalClass]
public partial class HostLobbyButton : LobbyTypeButton
{
    [Export] private int _serverPort = 8080;
    private ENetMultiplayerPeer _peer = new();
    
    protected override void OnPressed()
    {
        MenuPanel lobbyPanel = this.LobbyPanelScene.Instantiate<MenuPanel>();
        Error error = this._peer.CreateServer(_serverPort, 4);
        if (error != Error.Ok)
        {
            throw new Exception(error.ToString());
        }
        this.GetTree().GetMultiplayer().MultiplayerPeer = this._peer;
        ServerDiscoveryResponder.StartRespondingDiscoveryRequests(_serverPort);
        this.EmitSignalRequestNewPanel(lobbyPanel);
    }
}