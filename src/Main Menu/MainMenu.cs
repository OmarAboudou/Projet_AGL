using System.Collections.Generic;
using Godot;

namespace Main_Menu;

[GlobalClass]
public partial class MainMenu : PanelContainer
{
    [Export] private Control _menuPanelContainer;
    [Export] private MenuGoBackButton _menuGoBackButton;
    [Export] private PackedScene _initialMenuPanelScene;
    
    private readonly Stack<MenuPanel> _menuPanelStack = new();

    public override void _Ready()
    {
        base._Ready();
        this.UpdateGoBackButtonState();
        this.AddNewPanel(this._initialMenuPanelScene.Instantiate<MenuPanel>());
        this._menuGoBackButton.Pressed += this.TryGoBack;
    }
    
    private void AddNewPanel(MenuPanel newPanel)
    {
        if(!this.IsMultiplayerAuthority()) return;
        
        if (this._menuPanelStack.TryPeek(out MenuPanel currentPanel))
        {
            currentPanel.RequestNewPanel -= this.AddNewPanel;
            currentPanel.RequestGoBack -= this.TryGoBack;
            currentPanel.Hide();
        }
        _menuPanelStack.Push(newPanel);
        _menuPanelContainer.AddChild(newPanel);
        newPanel.RequestNewPanel += this.AddNewPanel;
        newPanel.RequestGoBack += this.TryGoBack;
        
        this.UpdateGoBackButtonState();
    }

    private void TryGoBack()
    {
        if (this.CanGoBack())
        {
            this.GoBack();
        }
    }

    private bool CanGoBack()
    {
        return this._menuPanelStack.Count > 1;
    }

    private void GoBack()
    {
        MenuPanel currentPanel = this._menuPanelStack.Pop();
        currentPanel.QueueFree();
        
        MenuPanel newCurrentPanel = this._menuPanelStack.Peek();
        newCurrentPanel.Show();
        
        this.UpdateGoBackButtonState();
    }
    
    private void UpdateGoBackButtonState()
    {
        this._menuGoBackButton.Disabled = this._menuPanelStack.Count <= 1;
    }
    
    
    
}