using System.Linq;
using Game.Mini_Games;
using Game.Players;
using Godot;

namespace Game.Loading_Screen;

public partial class MiniGameLoadingScreen : Control
{
    [Export] private PackedScene _miniGameLoadingScreenPlayerSlotScene;
    [Export] private Control _miniGameLoadingScreenPlayerSlotContainer;
    [Export] private Label _miniGameNameLabel;
    [Export] private RichTextLabel _miniGameDescriptionLabel;
    [Export] private TextureRect _miniGameTextureRect;
    
    private MiniGameData _miniGameData;
    private PlayerData[] _playerDatas;
    
    private MiniGameLoadingScreenPlayerSlot[] _playerSlots;

    public override void _Ready()
    {
        base._Ready();
        this.UpdateMiniGameUI();
        this.UpdatePlayerDatasUI();
    }

    public void UpdateMiniGameUI()
    {
        this._miniGameNameLabel.Text = this._miniGameData.Name;
        this._miniGameDescriptionLabel.Text = this._miniGameData.Description;
        this._miniGameTextureRect.Texture = this._miniGameData.Texture2D;
    }

    public void UpdatePlayerDatasUI()
    {
        foreach (Node child in this._miniGameLoadingScreenPlayerSlotContainer.GetChildren())
        {
            child.QueueFree();
        }
        foreach (PlayerData playerData in this._playerDatas)
        {
            MiniGameLoadingScreenPlayerSlot playerSlot = this._miniGameLoadingScreenPlayerSlotScene.Instantiate<MiniGameLoadingScreenPlayerSlot>();
            playerSlot.PlayerData = playerData;
            this._miniGameLoadingScreenPlayerSlotContainer.AddChild(playerSlot);
        }
        this._playerSlots = this._miniGameLoadingScreenPlayerSlotContainer.GetChildren().Where(c =>  c is MiniGameLoadingScreenPlayerSlot).Cast<MiniGameLoadingScreenPlayerSlot>().ToArray();
    }
}