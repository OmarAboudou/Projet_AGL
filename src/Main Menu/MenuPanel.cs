using Godot;

namespace Main_Menu;

[GlobalClass]
public partial class MenuPanel : Control
{
    [Signal]
    public delegate void AddNewMenuPanelEventHandler(MenuPanel newMenuPanel);
    
    [Signal]
    public delegate void GoBackToPreviousMenuPanelEventHandler();
    
    [Signal]
    public delegate void GoBackToFirstMenuPanelEventHandler();
}