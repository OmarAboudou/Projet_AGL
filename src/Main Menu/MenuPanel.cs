using Godot;

namespace Main_Menu;

[GlobalClass]
public abstract partial class MenuPanel : PanelContainer
{
    public virtual void OnGoBack() { }
    
    [Signal]
    public delegate void RequestGoBackEventHandler();
    
    [Signal]
    public delegate void RequestNewPanelEventHandler(MenuPanel newPanel);
}