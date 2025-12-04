using Godot;

namespace Main_Menu;

[GlobalClass]
public abstract partial class MainMenuPanel : PanelContainer
{
    [Signal]
    public delegate void RequestNewPanelEventHandler(MainMenuPanel newPanel);
}