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
        this.IsOccupied = false;
        this._emptySlotControl.Show();
        this._nonEmptySlotControl.Hide();
        this._playerNameLineEdit.SetText(this._defaultPlayerName);
    }

    [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable, CallLocal = true)]
    public void SetPeer(int peerId)
    {
        this.SetMultiplayerAuthority(peerId);
        this.IsOccupied = true;
        this._emptySlotControl.Hide();
        this._nonEmptySlotControl.Show();
        this.UpdateEditableState();
    }

    public void ClearPeer()
    {
        this.SetMultiplayerAuthority(1);
        this.IsOccupied = false;
        this._emptySlotControl.Show();
        this._nonEmptySlotControl.Hide();
        this.UpdateEditableState();
    }

    private void UpdateEditableState()
    {
        bool isAuthority = this.IsAuthority();
        this._playerNameLineEdit.Editable = isAuthority;
        this._readyButton.MouseFilter = isAuthority ? MouseFilterEnum.Stop : MouseFilterEnum.Ignore;
    }

    private bool IsAuthority()
    {
        return this.GetMultiplayerAuthority() == Multiplayer.MultiplayerPeer.GetUniqueId();
    }
}