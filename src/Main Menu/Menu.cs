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
        this.UpdateButtonState();
        this._goBackButton.Pressed += GoBackButtonOnPressed;
        this._menuPanelManager.WentBackToPreviousMenuPanel += MenuPanelManagerOnWentBackToPreviousMenuPanel;
        this._menuPanelManager.NewMenuPanelAdded += MenuPanelManagerOnNewMenuPanelAdded;
    }

    private void MenuPanelManagerOnWentBackToPreviousMenuPanel()
    {
        this.UpdateButtonState();
    }

    private void MenuPanelManagerOnNewMenuPanelAdded(MenuPanel newMenuPanel)
    {
        this.UpdateButtonState();
    }

    private void GoBackButtonOnPressed()
    {
        this._menuPanelManager.GoBackToPreviousMenuPanel();
    }

    private void UpdateButtonState()
    {
        this._goBackButton.Disabled = !this._menuPanelManager.CanGoBackToPreviousMenuPanel();
    }
    
}