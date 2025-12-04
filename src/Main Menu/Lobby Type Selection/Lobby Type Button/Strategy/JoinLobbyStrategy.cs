using System;
using Godot;
using Server;

namespace Main_Menu.Lobby_Type_Selection.Lobby_Type_Button.Strategy;

[GlobalClass]
public partial class JoinLobbyStrategy : LobbyTypeButtonStrategy
{
    private bool _isSearching;
    private double _serverDiscoveryTimeoutInSec = 5;
    private SceneTreeTimer _serverDiscoveryTimer;
    
    public override void Execute()
    {
        ServerDiscoveryRequester.OnServerDiscovered += ServerDiscoveryRequesterOnOnServerDiscovered;
        ServerDiscoveryRequester.SearchServer();
        this._serverDiscoveryTimer = this.GetTree().CreateTimer(_serverDiscoveryTimeoutInSec);
        this._serverDiscoveryTimer.Timeout += this.OnTimeout;
    }

    private void OnTimeout()
    {
        ServerDiscoveryRequester.StopSearchingServer();
        GD.PushWarning($"Couldn't find a server in {_serverDiscoveryTimeoutInSec} seconds.");
    }

    private void ServerDiscoveryRequesterOnOnServerDiscovered(string ip, int port)
    {
        this._serverDiscoveryTimer.Timeout -= this.OnTimeout;
        this._serverDiscoveryTimer.SetTimeLeft(0);

        ENetMultiplayerPeer peer = new();
        Error error = peer.CreateClient(ip, port);
        if (error != Error.Ok)
        {
            throw new Exception(error.ToString());
        }
        GD.Print($"Successfully connected to {ip}:{port}");
        this.Multiplayer.MultiplayerPeer = peer;
    }
}