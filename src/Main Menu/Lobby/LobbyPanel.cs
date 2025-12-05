using Godot;

namespace Main_Menu.Lobby;

[GlobalClass]
public partial class LobbyPanel : MenuPanel
{
    private ENetMultiplayerPeer _multiplayerPeer;
    private SceneMultiplayer _sceneMultiplayer;
    
    public override void _Ready()
    {
        base._Ready();
        this._multiplayerPeer = (ENetMultiplayerPeer)this.GetMultiplayer().MultiplayerPeer;
        this._sceneMultiplayer = (SceneMultiplayer)this.GetMultiplayer();
        this._sceneMultiplayer.ServerDisconnected += OnServerDisconnected;
        GD.Print("Player : " + this._multiplayerPeer.GetUniqueId());
        // TODO : Create a Lobby Player with the current peer id.
    }

    private void OnServerDisconnected()
    {
        this.EmitSignalRequestGoBack();
    }

    public override void OnGoBack()
    {
        base.OnGoBack();
        foreach (int peer in this.GetMultiplayer().GetPeers())
        {
            this._multiplayerPeer.DisconnectPeer(peer);            
        }
        this.GetMultiplayer().MultiplayerPeer = null;
    }
    
    
}