using Godot;

namespace Menu;

[GlobalClass]
public abstract partial class MenuPanel : Control
{
    [Signal] 
    public delegate void NewMenuPanelRequestedEventHandler(MenuPanelData newMenuPanelData);
    
}