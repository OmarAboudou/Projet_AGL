using Godot;
using Main_Menu.Lobby;

namespace Main_Menu.Lobby_Type_Selection.Lobby_Type_Button.Strategy;

[GlobalClass]
public abstract partial class LobbyTypeButtonStrategy : Node
{
    public abstract void Execute();
    
    [Signal]
    public delegate void LobbyTypeChosenEventHandler(LobbyPanel lobbyPanel);
}