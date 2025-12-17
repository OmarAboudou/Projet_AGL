using System;
using Godot;
using Main_Menu.Lobby;
using Server;

namespace Main_Menu.Server_Selection;

[GlobalClass]
public partial class ServerSelectionPanel : MenuPanel
{
    [Export] private Button _refreshButton;
    [Export] private Control _searchingIndicator;
    [Export] private ServerEntriesManager _serverEntriesManager;
    [Export] private float _searchingDurationInSeconds = 5;
    private SceneTreeTimer _serverSearchingTimer;
    [Export] private PackedScene _lobbyPanelScene;
    
    public override void _Ready()
    {
        base._Ready();
        this._refreshButton.Pressed += this.SearchServer;
        this._serverEntriesManager.ServerEntrySelected += ServerEntriesManagerOnServerEntrySelected;
    }

    public override void OnPanelIsCurrent()
    {
        base.OnPanelIsCurrent();
        this.SearchServer();
    }

    private void ServerEntriesManagerOnServerEntrySelected(string ipAddress, int port)
    {
        LobbyPanel lobbyPanel = this._lobbyPanelScene.Instantiate<LobbyPanel>();
        ENetMultiplayerPeer clientPeer = new();
        Error error = clientPeer.CreateClient(ipAddress, port);
        if (error != Error.Ok)
        {
            throw new Exception(error.ToString());
        }
        Multiplayer.MultiplayerPeer = clientPeer;
        this.EmitSignalAddNewMenuPanel(lobbyPanel);
    }

    private void SearchServer()
    {
        if(this.IsSearchingServer()) return;
        
        this._serverSearchingTimer = this.GetTree().CreateTimer(this._searchingDurationInSeconds);
        
        this._refreshButton.Disabled = true;
        this._searchingIndicator.Visible = true;
        
        this._serverEntriesManager.ClearEntries();
        ServerDiscoveryRequester.OnServerDiscovered += this._serverEntriesManager.AddEntry;
        ServerDiscoveryRequester.SearchServer();
        
        this._serverSearchingTimer.Timeout += this.DoneSearchingServer;

    }

    private void DoneSearchingServer()
    {
        ServerDiscoveryRequester.StopSearchingServer();
        ServerDiscoveryRequester.OnServerDiscovered -= this._serverEntriesManager.AddEntry;
        
        this._refreshButton.Disabled = false;
        this._searchingIndicator.Visible = false;

        this._serverSearchingTimer?.SetTimeLeft(0);
        this._serverSearchingTimer = null;
    }

    private bool IsSearchingServer()
    {
        return this._serverSearchingTimer != null;
    }
}