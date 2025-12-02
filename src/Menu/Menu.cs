using System.Collections.Generic;
using Godot;

namespace Menu;

public partial class Menu : Control
{
    [Export] private MenuPanelData _initialMenuPanelData;
    [Export] private Label _menuNameLabel;
    [Export] private Button _goBackButton;

    public override void _Ready()
    {
        base._Ready();
        this.StackMenuScene(this._initialMenuPanelData);
        this._goBackButton.Pressed += GoBackButtonOnPressed;
    }

    [Export] private Control _menuContainer;
    private Stack<MenuPanelData> _menuDataStack = new();
    private Stack<MenuPanel> _menuSceneRootStack = new();
    
    private void StackMenuScene(MenuPanelData menuData)
    {
        MenuPanel current = this._menuSceneRootStack.Count > 0 ? this._menuSceneRootStack.Peek() : null;
        if (current != null)
        {
            current.Hide();
            current.NewMenuPanelRequested -= CurrentOnNewMenuPanelRequested;
        }
        MenuPanel instance = menuData.MenuPanelScene.Instantiate<MenuPanel>();
        instance.NewMenuPanelRequested += CurrentOnNewMenuPanelRequested;
        this._menuSceneRootStack.Push(instance);
        this._menuDataStack.Push(menuData);
        instance.Show();
        this._menuContainer.AddChild(instance);
        this._menuNameLabel.Text = menuData.MenuName;
    }
    
    private void GoBackButtonOnPressed()
    {
        if(!this.CanGoBack()) return;
        
        
    }

    private bool CanGoBack()
    {
        return this._menuSceneRootStack.Count > 1;
    }

    private void CurrentOnNewMenuPanelRequested(MenuPanelData newMenuPanelData)
    {
        this.StackMenuScene(newMenuPanelData);
    }
}