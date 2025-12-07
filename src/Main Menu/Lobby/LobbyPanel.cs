using Godot;

namespace Main_Menu.Lobby;

[GlobalClass]
public partial class LobbyPanel : MenuPanel
{
    [Export] private PlayerSlotsManager _playerSlotsManager;

    public override void _Ready()
    {
        base._Ready();
        if (this.IsMultiplayerAuthority())
        {
            Multiplayer.PeerConnected += MultiplayerOnPeerConnected;
            Multiplayer.PeerDisconnected += MultiplayerOnPeerDisconnected;
        }
    }

    private void MultiplayerOnPeerConnected(long id)
    {
        _playerSlotsManager.AddPeer((int)id);
    }
    
    private void MultiplayerOnPeerDisconnected(long id)
    {
        _playerSlotsManager.RemovePeer((int)id);
    }

    public void AddPeer(long id)
    {
        this._playerSlotsManager.AddPeer((int)id);
    }

    private void RemovePeer(long id)
    {
        this._playerSlotsManager.RemovePeer((int)id);
    }
}