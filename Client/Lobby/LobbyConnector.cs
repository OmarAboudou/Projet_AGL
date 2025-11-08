using System;
using Godot;
using Project_AGL.Client.Server_Discovery;
using Project_AGL.Common.Composition_System;

namespace Project_AGL.Client.Lobby;


/// <summary>
/// The role of this class is, for a client, to connect to a discovered Server
/// </summary>
public partial class LobbyConnector : Node
{
    [Export, Inject] private ServerDiscoveryRequester _serverDiscoveryRequester;
    
    private ENetMultiplayerPeer _peer = new();
    
    public override void _Ready()
    {
        base._Ready();
        
        this._serverDiscoveryRequester.ServerDiscovered += ServerDiscoveryRequesterOnServerDiscovered;
        this._serverDiscoveryRequester.SearchServer();
        
    }

    private void ServerDiscoveryRequesterOnServerDiscovered(string ipAddress, int port)
    {
        this._serverDiscoveryRequester.StopSearchingServer();
        Error e = this._peer.CreateClient(ipAddress, port);
        Multiplayer.MultiplayerPeer = _peer;

        Multiplayer.ConnectedToServer += OnConnectedToServer;
        Multiplayer.ConnectionFailed += OnConnectionFailed;
        Multiplayer.ServerDisconnected += OnServerDisconnected;
        if (e != Error.Ok)
        {
            throw new Exception(e.ToString());
        }
    }

    private void OnConnectedToServer()
    {
        GD.Print("Client connected to lobby");
    }

    private void OnServerDisconnected()
    {
        GD.Print("Client disconnected from lobby");
    }

    private void OnConnectionFailed()
    {
        GD.Print("Connection failed");
    }
}