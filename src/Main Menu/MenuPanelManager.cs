using System.Linq;
using Godot;

namespace Main_Menu;

[GlobalClass]
public partial class MenuPanelManager : Control
{
    [Export] private PackedScene _startMenuPanel;

    [Signal]
    public delegate void NewMenuPanelAddedEventHandler(MenuPanel newMenuPanel);
    
    [Signal]
    public delegate void WentBackToPreviousMenuPanelEventHandler();
    
    public override void _Ready()
    {
        base._Ready();
        MenuPanel initialMenuPanel = this._startMenuPanel.Instantiate<MenuPanel>();
        this.AddNewMenuPanel(initialMenuPanel);
    }

    public void AddNewMenuPanel(MenuPanel newMenuPanel)
    {
        MenuPanel currentMenuPanel = GetChildOrNull<MenuPanel>(-1);
        if (currentMenuPanel != null)
        {
            this.DeactivateMenuPanel(currentMenuPanel);
        }
        this.ActivateMenuPanel(newMenuPanel);
        this.AddChild(newMenuPanel);
        this.EmitSignalNewMenuPanelAdded(newMenuPanel);
    }

    public void GoBackToPreviousMenuPanel()
    {
        MenuPanel[] currentPanels = this.GetChildren().OfType<MenuPanel>().ToArray();
        if (currentPanels.Length > 1)
        {
            MenuPanel currentPanel = currentPanels[^1];
            currentPanel.OnGoBackFrom();
            this.DeactivateMenuPanel(currentPanel);
            currentPanel.QueueFree();
        }
        MenuPanel newCurrentPanel = currentPanels[^2];
        this.ActivateMenuPanel(newCurrentPanel);
        this.EmitSignalWentBackToPreviousMenuPanel();
    }

    public void GoBackToFirstMenuPanel()
    {
        int numberOfPanels = this.GetChildren().OfType<MenuPanel>().Count();
        for (int i = 0; i < numberOfPanels-1; i++)
        {
            this.GoBackToPreviousMenuPanel();
        }
    }

    public bool CanGoBackToPreviousMenuPanel()
    {
         return this.GetChildren().OfType<MenuPanel>().Count() > 1;
    }

    private void ActivateMenuPanel(MenuPanel menuPanel)
    {
        menuPanel.AddNewMenuPanel += this.AddNewMenuPanel;
        menuPanel.GoBackToPreviousMenuPanel += this.GoBackToPreviousMenuPanel;
        menuPanel.GoBackToFirstMenuPanel += this.GoBackToFirstMenuPanel;

        if (!menuPanel.IsNodeReady())
        {
            menuPanel.Ready += MenuPanelOnReady;

            void MenuPanelOnReady()
            {
                menuPanel.Ready -= MenuPanelOnReady;
                menuPanel.OnPanelIsCurrent();
            }    
        }
        else
        {
            menuPanel.OnPanelIsCurrent();
        }
        
        menuPanel.Show();
    }

    private void DeactivateMenuPanel(MenuPanel menuPanel)
    {
        menuPanel.AddNewMenuPanel -= this.AddNewMenuPanel;
        menuPanel.GoBackToPreviousMenuPanel -= this.GoBackToPreviousMenuPanel;
        menuPanel.GoBackToFirstMenuPanel -= this.GoBackToFirstMenuPanel;
        menuPanel.Hide();
    }
}