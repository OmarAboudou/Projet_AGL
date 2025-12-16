using Godot;

namespace Main_Menu.Server_Selection;

[GlobalClass]
public partial class ServerEntriesManager : Control
{
    [Export] private PackedScene _serverEntryScene;
    [Export] private Control _serverEntryRootNode;
    [Signal]
    public delegate void ServerEntrySelectedEventHandler(string ipAddress, int port);
    
    public void ClearEntries()
    {
        foreach (Node child in this._serverEntryRootNode.GetChildren())
        {
            if (child is ServerEntry serverEntry)
            {
                serverEntry.ServerEntrySelected -= this.EmitSignalServerEntrySelected;
                serverEntry.QueueFree();
            }
        }
    }

    public void AddEntry(string ipAddress, int port)
    {
        ServerEntry serverEntry = this._serverEntryScene.Instantiate<ServerEntry>();
        serverEntry.Initialize(ipAddress, port);
        serverEntry.ServerEntrySelected += this.EmitSignalServerEntrySelected;
        this._serverEntryRootNode.AddChild(serverEntry);
    }

}