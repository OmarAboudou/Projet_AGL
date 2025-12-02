using Godot;

namespace Menu;

[Tool, GlobalClass]
public partial class MenuPanelData : Resource
{
    [Export] public string MenuName { get; private set; }
    [Export] public MenuPanelScene MenuPanelScene { get; private set; }
}