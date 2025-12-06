using Godot;

namespace Main_Menu;

[GlobalClass]
public partial class MenuGoBackButton : MarginContainer
{
    [Export] private BaseButton _innerButton;
    
    public override void _Ready()
    {
        base._Ready();
        this._innerButton.Pressed += this.EmitSignalPressed;
    }

    public bool Disabled
    {
        get => this._innerButton.Disabled;
        set => this._innerButton.Disabled = value;
    }

    [Signal] public delegate void PressedEventHandler();
}