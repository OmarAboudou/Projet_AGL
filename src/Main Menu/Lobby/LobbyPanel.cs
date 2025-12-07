using System.Linq;
using Godot;

namespace Main_Menu.Lobby;

[GlobalClass]
public partial class LobbyPanel : MenuPanel
{
    [Export] private PlayerSlotsManager _playerSlotsManager;

    public override void OnGoBackFrom()
    {
        base.OnGoBackFrom();
        Multiplayer.MultiplayerPeer.Close();
        Multiplayer.MultiplayerPeer = null;
    }

    public override void _Ready()
    {
        base._Ready();
        if (this.IsMultiplayerAuthority())
        {
            Multiplayer.PeerConnected += this.ServerOnPeerConnected;
            Multiplayer.PeerDisconnected += this.ServerOnPeerDisconnected;
            this.ServerOnPeerConnected(1);
        }
        else
        {
            Multiplayer.ServerDisconnected += this.ClientOnServerDisconnected;
        }
    }

    private void ClientOnServerDisconnected()
    {
        this.EmitSignalGoBackToPreviousMenuPanel();
    }

    private void ServerOnPeerConnected(long id)
    {
        this._playerSlotsManager.UpdateAuthorities(this._playerSlotsManager.PlayerSlots.Select(p => p.GetMultiplayerAuthority()).ToArray());
        this._playerSlotsManager.AddPeer((int)id);
    }

    private void ServerOnPeerDisconnected(long id)
    {
        this._playerSlotsManager.UpdateAuthorities(this._playerSlotsManager.PlayerSlots.Select(p => p.GetMultiplayerAuthority()).ToArray());
        this._playerSlotsManager.RemovePeer((int)id);
    }
}