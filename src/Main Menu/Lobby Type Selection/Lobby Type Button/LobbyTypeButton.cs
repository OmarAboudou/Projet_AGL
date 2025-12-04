using Godot;
using Main_Menu.Lobby_Type_Selection.Lobby_Type_Button.Strategy;
using Main_Menu.Lobby;

namespace Main_Menu.Lobby_Type_Selection.Lobby_Type_Button;

[GlobalClass]
public partial class LobbyTypeButton : PanelContainer
{
    [Export] private BaseButton _innerButton;
    [Export] private LobbyTypeButtonStrategy _strategy;
    public override void _Ready()
    {
        base._Ready();
        this._innerButton.Pressed += this._strategy.Execute;
        this._strategy.LobbyTypeChosen += this.EmitSignalLobbyTypeChosen;
    }

    [Signal]
    public delegate void LobbyTypeChosenEventHandler(LobbyPanel lobbyPanel);
    
}