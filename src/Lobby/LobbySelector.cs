using System;
using System.Linq;
using Godot;
using Project_AGL.Client.Server_Discovery;
using Project_AGL.Common.Composition_System;
using Project_AGL.Server.Server_Discovery;
using Project_AGL.Shared.Lobby;

namespace Project_AGL.Lobby;

using static LobbyConstants;

public partial class LobbySelector : Node
{
    [Export, Inject] private ServerDiscoveryRequester _serverDiscoveryRequester;
    [Export, Inject] private ServerDiscoveryResponder _serverDiscoveryResponder;
    private ENetMultiplayerPeer _peer = new();
    private bool _alreadySet = false;

    public override void _Ready()
    {
        base._Ready();
        this.Multiplayer.PeerConnected += id => GD.Print($"Peer connected: {id}");
        this.Multiplayer.PeerDisconnected += id => GD.Print($"Peer disconnected: {id}");
        this.Multiplayer.ConnectedToServer += () => GD.Print("Connected to server");
        this.Multiplayer.ConnectionFailed += () => GD.Print("Connection failed");
        this.Multiplayer.ServerDisconnected += () => GD.Print("Server disconnected");
    }

    public void HostLobby()
    {
        if(this._alreadySet) return;
        this._alreadySet = true;
        Error e = this._peer.CreateServer(PORT, MAX_PLAYER);

        if (e != Error.Ok)
        {
            throw new Exception($"Failed to create server : {e.ToString()}");
        }

        String address = IP.GetLocalAddresses().FirstOrDefault("localhost");
        this._serverDiscoveryResponder.StartRespondingDiscoveryRequests(address, 8080);
        this.Multiplayer.MultiplayerPeer = this._peer;
        GD.Print("Server created");
    }

    public void JoinLobby()
    {
        if(this._alreadySet) return;
        this._alreadySet = true;
        this._serverDiscoveryRequester.ServerDiscovered += this.ServerDiscoveryRequesterOnServerDiscovered;
        this._serverDiscoveryRequester.SearchServer();
        
    }

    private void ServerDiscoveryRequesterOnServerDiscovered(string ipAddress, int port)
    {
        this._serverDiscoveryRequester.StopSearchingServer();
        Error e = this._peer.CreateClient(ipAddress, port);
        if (e != Error.Ok)
        {
            throw new Exception($"Failed to create client : {e.ToString()}");
        }
        this.Multiplayer.MultiplayerPeer = this._peer;
        
    }
}