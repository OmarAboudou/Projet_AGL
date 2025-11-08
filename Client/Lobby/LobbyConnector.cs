using System;
using Godot;
using Project_AGL.Client.Server_Discovery;
using Project_AGL.Common.Composition_System;

namespace Project_AGL.Client.Lobby;

public partial class LobbyConnector : Node
{
    [Export, Inject] private ServerDiscoveryRequester _serverDiscoveryRequester;
    
    private ENetMultiplayerPeer peer = new();
    
    public override void _Ready()
    {
        base._Ready();
        this._serverDiscoveryRequester.ServerDiscovered += ServerDiscoveryRequesterOnServerDiscovered;
        this._serverDiscoveryRequester.SearchServer();
        
    }

    private void ServerDiscoveryRequesterOnServerDiscovered(string ipAddress, int port)
    {
        this._serverDiscoveryRequester.StopSearchingServer();
        Error e = this.peer.CreateClient(ipAddress, port);
        if (e != Error.Ok)
        {
            throw new Exception(e.ToString());
        }
        GD.Print("Client connected to lobby");
    }
}