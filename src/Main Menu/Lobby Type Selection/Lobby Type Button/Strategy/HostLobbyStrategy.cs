using System;
using Godot;
using Server;

namespace Main_Menu.Lobby_Type_Selection.Lobby_Type_Button.Strategy;

[GlobalClass]
public partial class HostLobbyStrategy : LobbyTypeButtonStrategy
{
    [Export] private PackedScene _lobbyPanelScene;
    [Export] private int _serverPort = 8080;
    private ENetMultiplayerPeer _peer = new();
    
    public override void Execute()
    {
        MenuPanel lobbyPanel = this._lobbyPanelScene.Instantiate<MenuPanel>();
        Error error = this._peer.CreateServer(_serverPort, 4);
        if (error != Error.Ok)
        {
            throw new Exception(error.ToString());
        }
        this.Multiplayer.MultiplayerPeer = this._peer;
        ServerDiscoveryResponder.StartRespondingDiscoveryRequests(_serverPort);
        this.EmitSignalRequestNewPanel(lobbyPanel);
    }

}