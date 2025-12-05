using Godot;

namespace Main_Menu;

[GlobalClass]
public partial class MenuPanel : PanelContainer
{
    [Signal]
    public delegate void RequestGoBackEventHandler();
    
    [Signal]
    public delegate void RequestNewPanelEventHandler(MenuPanel newPanel);
}