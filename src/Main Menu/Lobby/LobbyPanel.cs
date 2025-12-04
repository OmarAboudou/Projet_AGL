using Godot;

namespace Main_Menu.Lobby;

[GlobalClass]
public partial class LobbyPanel : MainMenuPanel
{
    [Export] private Label _label;
    
    public override void _Ready()
    {
        base._Ready();
        this._label.Text = "";
        this.Multiplayer.PeerConnected += MultiplayerOnPeerConnected;
        this.Multiplayer.PeerDisconnected += MultiplayerOnPeerDisconnected;
        this.MultiplayerOnPeerConnected(this.Multiplayer.GetUniqueId());
    }

    private void MultiplayerOnPeerConnected(long id)
    {
        this._label.Text += $"Welcome peer with id : {id}\n";
    }

    private void MultiplayerOnPeerDisconnected(long id)
    {
        this._label.Text += $"Good bye peer with id : {id}\n";
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        this.Multiplayer.PeerConnected -= MultiplayerOnPeerConnected;
        this.Multiplayer.PeerDisconnected -= MultiplayerOnPeerDisconnected;
    }
    
}