using System.Collections.Generic;
using Common.Utils;
using Godot;

namespace Menu_System;

[GlobalClass]
public partial class Menu : Control
{
    [Export] private PackedScene _initialMenuScene;
    [Export] private Control _menuPanelContainer;
    [Export] private BaseButton _goBackButton;
    
    private Stack<MenuPanel> _menuPanelStack = new();

    public override void _Ready()
    {
        base._Ready();
        this._goBackButton.Pressed += this.GoBackButtonOnPressed;
        this.AddNewMenuPanel(this._initialMenuScene.Instantiate<MenuPanel>());
    }

    private void AddNewMenuPanel(MenuPanel menuPanel)
    {
        if (this._menuPanelStack.TryPeek(out MenuPanel currentMenuPanel))
        {
            this.DisableMenuPanel(currentMenuPanel);
        }
        this.EnableMenuPanel(menuPanel);
        this._menuPanelStack.Push(menuPanel);
        this._menuPanelContainer.AddChild(menuPanel);

        this.UpdateGoBackButtonState();
    }

    private void RemoveCurrentMenuPanel()
    {
        MenuPanel menuPanelToRemove = this._menuPanelStack.Pop();
        menuPanelToRemove.QueueFree();
        
        MenuPanel newCurrentMenuPanel = this._menuPanelStack.Peek();
        this.EnableMenuPanel(newCurrentMenuPanel);
        
        this.UpdateGoBackButtonState();
    }
    
    private void CurrentMenuPanelOnRequestNewPanel(MenuPanel newPanel)
    {
        this.AddNewMenuPanel(newPanel);
    }
    
    private void UpdateGoBackButtonState()
    {
        this._goBackButton.Disabled = this._menuPanelStack.Count <= 1;   
    }
    private void GoBackButtonOnPressed()
    {
        this.RemoveCurrentMenuPanel();
    }

    private void EnableMenuPanel(MenuPanel menuPanel)
    {
        menuPanel.SetAllProcessing(true);
        menuPanel.RequestNewPanel += this.CurrentMenuPanelOnRequestNewPanel;
    }

    private void DisableMenuPanel(MenuPanel menuPanel)
    {
        menuPanel.SetAllProcessing(false);
        menuPanel.RequestNewPanel -= this.CurrentMenuPanelOnRequestNewPanel;
    }
    
}