using Game.Players;
using Godot;

namespace Game.Loading_Screen;

public partial class MiniGameLoadingScreenPlayerSlot : Control
{
    [Export] private Label _playerNameLabel;
    [Export] private ColorRect _playerColorRect;
    private PlayerData _playerData;

    [Export]
    public PlayerData PlayerData
    {
        get => this._playerData;
        set
        {
            this._playerData = value;
            this.Update();
        }
    }

    public override void _Ready()
    {
        base._Ready();
        this.Update();
    }

    private void Update()
    {
        this._playerNameLabel?.SetText(this.PlayerData.PlayerName);
        this._playerColorRect?.SetColor(this.PlayerData.PlayerColor);
    }

}