using Godot;
using Main_Menu;

[GlobalClass]
public partial class LobbyTypeSelectionPanel : MenuPanel
{
    [Export] private Button _joinButton;
    [Export] private Button _hostButton;

    public override void _Ready()
    {
        base._Ready();
        this._joinButton.Pressed += JoinButtonOnPressed;
        this._hostButton.Pressed += HostButtonOnPressed;
    }

    private void JoinButtonOnPressed()
    {
        
    }

    private void HostButtonOnPressed()
    {
        
    }
}
