using Godot;

namespace Main_Menu.Lobby;

[GlobalClass]
public partial class PlayerSlot : Control
{
    [Export] private Control _emptySlotControl;
    [Export] private Control _nonEmptySlotControl;
    [Export] private LineEdit _playerNameLineEdit;
    [Export] private Button _readyButton;
    [Export] private string _defaultPlayerName;
    
    [Export]
    public bool IsOccupied { get; private set; }

    public override void _Ready()
    {
        base._Ready();
        this._playerNameLineEdit.SetText(this._defaultPlayerName);
        this.IsOccupied = false;
        this._emptySlotControl.Show();
        this._nonEmptySlotControl.Hide();
    }

    public void SetPeer(int peerId)
    {
        this.SetMultiplayerAuthority(peerId);
        this.ShowConnected();
        this.UpdateEditableState();
    }

    public void ClearPeer()
    {
        this.SetMultiplayerAuthority(1);
        this.ShowEmpty();
    }

    private void ShowConnected()
    {        
        this.IsOccupied = true;
        this._emptySlotControl.Hide();
        this._nonEmptySlotControl.Show();
    }

    private void ShowEmpty()
    {
        this.IsOccupied = false;
        this._emptySlotControl.Show();
        this._nonEmptySlotControl.Hide();        
    }

    private void UpdateEditableState()
    {
        bool isAuthority = this.IsAuthority();
        this._playerNameLineEdit.Editable = isAuthority;
        this._playerNameLineEdit.MouseFilter = isAuthority ? MouseFilterEnum.Stop : MouseFilterEnum.Ignore;
        this._readyButton.MouseFilter = isAuthority ? MouseFilterEnum.Stop : MouseFilterEnum.Ignore;
    }

    private bool IsAuthority()
    {
        return this.GetMultiplayerAuthority() == Multiplayer.MultiplayerPeer.GetUniqueId();
    }
}