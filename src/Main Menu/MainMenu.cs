using System.Collections.Generic;
using Godot;

namespace Main_Menu;

[GlobalClass]
public partial class MainMenu : PanelContainer
{
    [Export] private Control _menuPanelContainer;
    [Export] private MenuGoBackButton _menuGoBackButton;
    [Export] private PackedScene _initialMenuPanelScene;
    
    private Stack<MainMenuPanel> _menuPanelStack = new();

    public override void _Ready()
    {
        base._Ready();
        this.UpdateGoBackButtonState();
        this.AddNewPanel(this._initialMenuPanelScene.Instantiate<MainMenuPanel>());
        this._menuGoBackButton.Pressed += this.MenuGoBack;
    }

    private void AddNewPanel(MainMenuPanel newPanel)
    {
        if (this._menuPanelStack.TryPeek(out MainMenuPanel currentPanel))
        {
            currentPanel.RequestNewPanel -= this.AddNewPanel;
            currentPanel.Hide();
        }
        _menuPanelStack.Push(newPanel);
        _menuPanelContainer.AddChild(newPanel);
        newPanel.RequestNewPanel += this.AddNewPanel;
        
        this.UpdateGoBackButtonState();
    }

    private void MenuGoBack()
    {
        MainMenuPanel currentPanel = this._menuPanelStack.Pop();
        currentPanel.QueueFree();
        
        MainMenuPanel newCurrentPanel = this._menuPanelStack.Peek();
        newCurrentPanel.Show();
        
        this.UpdateGoBackButtonState();
    }
    
    private void UpdateGoBackButtonState()
    {
        this._menuGoBackButton.Disabled = this._menuPanelStack.Count <= 1;
    }
    
    
    
}