using Godot;

namespace Main_Menu.Lobby;

[GlobalClass]
public partial class PlayerSlot : Control
{
    [Export] private Control _emptySlotControl;
    [Export] private Control _nonEmptySlotControl;
    [Export] private ColorPickerButton _colorPickerButton;
    [Export] private LineEdit _playerNameLineEdit;
    [Export] private Button _readyButton;
    
    [Export] private Color _defaultPlayerColor;
    [Export] private string _defaultPlayerName;

    [Export] public bool IsOccupied { get; private set; }
    
    [Signal] public delegate void ReadyStateChangedEventHandler(bool value);

    public override void _Ready()
    {
        base._Ready();
        this._readyButton.Toggled += this.EmitSignalReadyStateChanged;
        this.Reset();
    }

    public void SetPeer(int peerId)
    {
        this.SetMultiplayerAuthority(peerId);
        this.ShowConnected();
        this.UpdateEditableState();
    }

    public void ClearPeer()
    {
        this.Reset();
    }

    public PlayerData CreatePlayerData() => new(this.GetMultiplayerAuthority(), this._playerNameLineEdit.Text, this._colorPickerButton.Color);

    public bool IsReady() => this._readyButton.ButtonPressed;
    
    private void ShowConnected()
    {        
        this.IsOccupied = true;
        this._emptySlotControl.Hide();
        this._nonEmptySlotControl.Show();
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

    private void Reset()
    {
        this.SetMultiplayerAuthority(1);
        this._readyButton.ButtonPressed = false;
        this._playerNameLineEdit.SetText(this._defaultPlayerName);
        this._colorPickerButton.Color = this._defaultPlayerColor;
        this.IsOccupied = false;
        this._emptySlotControl.Show();
        this._nonEmptySlotControl.Hide();
    }
}