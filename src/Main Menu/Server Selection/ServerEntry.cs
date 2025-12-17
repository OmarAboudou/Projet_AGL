using System;
using Godot;

namespace Main_Menu.Server_Selection;

[GlobalClass]
public partial class ServerEntry : Control
{

    [Export] private Label _ipAddressLabel;
    [Export] private Label _portLabel;
    [Export] private Button _connectButton;
    
    [Signal]
    public delegate void ServerEntrySelectedEventHandler(string ipAddress, int port);
    
    public void Initialize(string ipAddress, int port)
    {
        this._ipAddressLabel.Text = ipAddress;
        this._portLabel.Text = port.ToString();
    }

    public override void _Ready()
    {
        base._Ready();
        this._connectButton.Pressed += ConnectButtonOnPressed;
    }

    private void ConnectButtonOnPressed()
    {
        this.EmitSignalServerEntrySelected(this._ipAddressLabel.Text, Convert.ToInt32(this._portLabel.Text));
    }
}