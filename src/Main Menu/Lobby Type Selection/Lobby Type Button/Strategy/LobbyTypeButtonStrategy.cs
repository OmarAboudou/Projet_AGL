using Godot;

namespace Main_Menu.Lobby_Type_Selection.Lobby_Type_Button.Strategy;

[GlobalClass]
public abstract partial class LobbyTypeButtonStrategy : Node
{
    public abstract void Execute();
    
    [Signal]
    public delegate void RequestNewPanelEventHandler(MenuPanel lobbyPanel);
}