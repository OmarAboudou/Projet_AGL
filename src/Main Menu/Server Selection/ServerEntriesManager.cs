using Godot;

namespace Main_Menu.Server_Selection;

[GlobalClass]
public partial class ServerEntriesManager : Control
{
    [Export] private PackedScene _serverEntryScene;
    [Export] private Control _serverEntryRootNode;


}