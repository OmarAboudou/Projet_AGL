using Godot;
using Godot.Collections;

namespace Main_Menu.Lobby;

[GlobalClass]
public partial class PlayerSlotsManager : Control
{
    public Array<PlayerSlot> PlayerSlots { get; private set; } = new();
    [Export] private Control _playerSlotsContainer;

    public override void _Ready()
    {
        base._Ready();
        foreach (Node child in this._playerSlotsContainer.GetChildren())
        {
            if (child is PlayerSlot playerSlot)
            {
                PlayerSlots.Add(playerSlot);
            }
        }
    }

    public void UpdateAuthorities(int[] authorities)
    {
        for (int i = 0; i < PlayerSlots.Count; i++)
        {
            int authority = authorities[i];
            Rpc(MethodName.SetSlotAuthority, i, authority);
        }
    }

    public void AddPeer(int peerId)
    {
        for(int i = 0; i < PlayerSlots.Count; i++)
        {
            PlayerSlot playerSlot = PlayerSlots[i];
            if (!playerSlot.IsOccupied)
            {
                Rpc(MethodName.AddPeerToSlot, peerId, i);
                return;
            }
        }
    }

    public void RemovePeer(int peerId)
    {
        for(int i = 0; i < PlayerSlots.Count; i++)
        {
           PlayerSlot playerSlots = PlayerSlots[i];
           if (playerSlots.GetMultiplayerAuthority() == peerId)
           {
               Rpc(MethodName.RemovePeerFromSlot, i);
               return;
           }
        }
    }

    [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable, CallLocal = true)]
    private void AddPeerToSlot(int peerId, int slotIndex)
    {
        PlayerSlots[slotIndex].SetPeer(peerId);
    }
    
    [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable, CallLocal = true)]
    private void RemovePeerFromSlot(int slotIndex)
    {
        PlayerSlots[slotIndex].ClearPeer();
    }
    
    [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable, CallLocal = true)]
    private void SetSlotAuthority(int slotIndex, int authorityPeerId)
    {
        PlayerSlots[slotIndex].SetMultiplayerAuthority(authorityPeerId);
    }
}