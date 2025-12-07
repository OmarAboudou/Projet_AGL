using System;
using Godot;
using Godot.Collections;

namespace Main_Menu.Lobby;

[GlobalClass]
public partial class PlayerSlotsManager : Control
{
    
    [Export] private Array<PlayerSlot> _playerSlots = new();

    public void AddPeer(int peerId)
    {
        foreach (PlayerSlot playerSlot in this._playerSlots)
        {
            if (!playerSlot.IsOccupied)
            {
                playerSlot.Rpc(PlayerSlot.MethodName.SetPeer, peerId);
                return;
            }

            int currentSlotPeerId = playerSlot.GetMultiplayerAuthority();
            playerSlot.RpcId(peerId, PlayerSlot.MethodName.SetPeer, currentSlotPeerId);
        }    
        throw new Exception($"No slots available for peer : {peerId}");
    }

    public void RemovePeer(int peerId)
    {
        foreach (PlayerSlot playerSlot in this._playerSlots)
        {
            if (playerSlot.GetMultiplayerAuthority() == peerId)
            {
                playerSlot.Rpc(PlayerSlot.MethodName.ClearPeer);
                return;
            }
        }
    }
    
}