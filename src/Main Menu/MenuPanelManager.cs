using System.Linq;
using Godot;

namespace Main_Menu;

[GlobalClass]
public partial class MenuPanelManager : Control
{
    [Export] private PackedScene _startMenuPanel;

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
            this.DisconnectFromMenuPanel(currentMenuPanel);
            currentMenuPanel.Hide();
        }
        this.ConnectToMenuPanel(newMenuPanel);
        this.AddChild(newMenuPanel);
        newMenuPanel.Show();
    }

    public void GoBackToPreviousMenuPanel()
    {
        MenuPanel[] currentPanels = this.GetChildren().OfType<MenuPanel>().ToArray();
        if (currentPanels.Length > 1)
        {
            MenuPanel currentPanel = currentPanels[^1];
            this.DisconnectFromMenuPanel(currentPanel);
            currentPanel.QueueFree();
        }
        MenuPanel newCurrentPanel = currentPanels[^2];
        this.ConnectToMenuPanel(newCurrentPanel);
    }

    public void GoBackToFirstMenuPanel()
    {
        int numberOfPanels = this.GetChildren().OfType<MenuPanel>().Count();
        for (int i = 0; i < numberOfPanels-1; i++)
        {
            this.GoBackToPreviousMenuPanel();
        }
    }

    private void ConnectToMenuPanel(MenuPanel menuPanel)
    {
        menuPanel.AddNewMenuPanel += this.AddNewMenuPanel;
        menuPanel.GoBackToPreviousMenuPanel += this.GoBackToPreviousMenuPanel;
        menuPanel.GoBackToFirstMenuPanel += this.GoBackToFirstMenuPanel;
    }

    private void DisconnectFromMenuPanel(MenuPanel menuPanel)
    {
        menuPanel.AddNewMenuPanel -= this.AddNewMenuPanel;
        menuPanel.GoBackToPreviousMenuPanel -= this.GoBackToPreviousMenuPanel;
        menuPanel.GoBackToFirstMenuPanel -= this.GoBackToFirstMenuPanel;
    }
}