using System;
using Godot;
using Main_Menu.Lobby;
using Main_Menu.Server_Selection;
using Server;

namespace Main_Menu.Lobby_Type_Selection;

[GlobalClass]
public partial class LobbyTypeSelectionPanel : MenuPanel
{
    [Export] private Button _joinButton;
    [Export] private Button _hostButton;
    [Export] private PackedScene _lobbyPanelScene;
    [Export] private PackedScene _serverSelectionPanelScene;
    private bool _isSearching;

    public override void _Ready()
    {
        base._Ready();
        this._joinButton.Pressed += this.JoinButtonOnPressed;
        this._hostButton.Pressed += this.HostButtonOnPressed;
    }

    private void JoinButtonOnPressed()
    {
        ServerSelectionPanel serverSelectionPanel = this._serverSelectionPanelScene.Instantiate<ServerSelectionPanel>();
        this.EmitSignalAddNewMenuPanel(serverSelectionPanel);
        /*if(this._isSearching) return;

        LobbyPanel lobbyPanel = this._lobbyPanelScene.Instantiate<LobbyPanel>();
        float timeoutTime = 3f;
        SceneTreeTimer timer = this.GetTree().CreateTimer(timeoutTime);
        ServerDiscoveryRequester.OnServerDiscovered += ServerDiscoveryRequesterOnOnServerDiscovered;
        timer.Timeout += TimerOnTimeout;
        ServerDiscoveryRequester.SearchServer();

        void TimerOnTimeout()
        {
            ServerDiscoveryRequester.StopSearchingServer();
            ServerDiscoveryRequester.OnServerDiscovered -= ServerDiscoveryRequesterOnOnServerDiscovered;
            timer.Timeout -= TimerOnTimeout;
            timer.TimeLeft = 0;
            this._isSearching = false;
            lobbyPanel.QueueFree();
            GD.PrintErr($"Couldn't find a server in {timeoutTime} seconds.");
        }

        void ServerDiscoveryRequesterOnOnServerDiscovered(string ip, int port)
        {

            ServerDiscoveryRequester.StopSearchingServer();
            ServerDiscoveryRequester.OnServerDiscovered -= ServerDiscoveryRequesterOnOnServerDiscovered;
            timer.Timeout -= TimerOnTimeout;
            timer.TimeLeft = 0;
            this._isSearching = false;

            ENetMultiplayerPeer clientPeer = new();
            Error error = clientPeer.CreateClient(ip, port);
            if (error != Error.Ok)
            {
                throw new Exception(error.ToString());
            }
            Multiplayer.MultiplayerPeer = clientPeer;
            this.EmitSignalAddNewMenuPanel(lobbyPanel);
        }*/
    }

    private void HostButtonOnPressed()
    {
        LobbyPanel lobbyPanel = this._lobbyPanelScene.Instantiate<LobbyPanel>();
        ENetMultiplayerPeer serverPeer = new();
        int port = 8081;
        Error error = serverPeer.CreateServer(port);
        if (error != Error.Ok)
        {
            throw new Exception(error.ToString());
        }
        Multiplayer.MultiplayerPeer = serverPeer;
        this.EmitSignalAddNewMenuPanel(lobbyPanel);
        ServerDiscoveryResponder.StartRespondingDiscoveryRequests(port);
    }
}