using Godot;

namespace Menu_System;

[GlobalClass]
public abstract partial class MenuPanel : Control
{
    [Signal] public delegate void RequestNewPanelEventHandler(MenuPanel newPanel);
}