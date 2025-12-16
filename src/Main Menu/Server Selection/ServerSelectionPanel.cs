using Godot;

namespace Main_Menu.Server_Selection;

[GlobalClass]
public partial class ServerSelectionPanel : MenuPanel
{
    [Export] private Button _refreshButton;
    [Export] private Control _searchingIndicator;
    [Export] private ServerEntriesManager _serverEntriesManager;

}