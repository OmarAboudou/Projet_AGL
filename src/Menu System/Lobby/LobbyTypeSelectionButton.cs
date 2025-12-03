using Godot;

namespace Menu_System.Lobby;

[GlobalClass]
public abstract partial class LobbyTypeSelectionButton : Button
{
    public override void _Ready()
    {
        base._Ready();
        this.Pressed += OnPressed;
    }

    [Signal] 
    public delegate void LobbyTypeChosenEventHandler(LobbyPanel lobbyPanel);

    protected abstract void OnPressed();
}