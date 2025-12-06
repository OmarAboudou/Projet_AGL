using Godot;

namespace Main_Menu;

[GlobalClass]
public partial class Menu : Control
{
    
    [Export] private Button _goBackButton;
    [Export] private MenuPanelManager _menuPanelManager;

    public override void _Ready()
    {
        base._Ready();
        this._goBackButton.Pressed += this._menuPanelManager.GoBackToPreviousMenuPanel;
    }
}