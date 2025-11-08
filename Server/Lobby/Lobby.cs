using System;
using System.Linq;
using Godot;
using Project_AGL.Common.Composition_System;
using Project_AGL.Server.Server_Discovery;
using Project_AGL.Shared.Lobby;

namespace Project_AGL.Server.Lobby;

using static LobbyConstants;

public partial class Lobby : Node
{
    [Export]
    [Inject]
    private ServerDiscoveryResponder _serverDiscoveryResponder;
    
    public override void _Ready()
    {
        base._Ready();
        ENetMultiplayerPeer peer = new();
        Error e = peer.CreateServer(PORT, MAX_PLAYER);
        if (e != Error.Ok)
        {
            throw new Exception(e.ToString());
        }

        string ip = IP.GetLocalAddresses().FirstOrDefault("localhost");
        GD.Print(ip);

        Multiplayer.MultiplayerPeer = peer;
        Multiplayer.PeerConnected += MultiplayerOnPeerConnected;
        Multiplayer.PeerDisconnected += MultiplayerOnPeerDisconnected;
        
        this._serverDiscoveryResponder.StartRespondingDiscoveryRequests(ip, PORT);

    }

    private void MultiplayerOnPeerDisconnected(long id)
    {
        
    }

    private void MultiplayerOnPeerConnected(long id)
    {
        GD.Print($"NEW PEER WITH ID {id}");
    }
    

}