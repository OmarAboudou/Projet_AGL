using Godot;

namespace Main_Menu.Lobby_Type_Selection.Lobby_Type_Button;

[GlobalClass]
public abstract partial class LobbyTypeButton : PanelContainer
{
    [Export] private Button _innerButton;
    [Export] protected PackedScene LobbyPanelScene;
    public override void _Ready()
    {
        base._Ready();
        this._innerButton.Pressed += this.OnPressed;
    }

    [Signal]
    public delegate void RequestNewPanelEventHandler(MenuPanel lobbyPanel);

    protected abstract void OnPressed();

}