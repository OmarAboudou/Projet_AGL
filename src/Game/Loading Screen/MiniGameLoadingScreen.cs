using System.Linq;
using Game.Mini_Games;
using Game.Players;
using Godot;
using Loading_Screen;

namespace Game.Loading_Screen;

public partial class MiniGameLoadingScreen : LoadingScreen
{
    [Export] private PackedScene _miniGameLoadingScreenPlayerSlotScene;
    [Export] private Control _miniGameLoadingScreenPlayerSlotContainer;
    [Export] private Label _miniGameNameLabel;
    [Export] private RichTextLabel _miniGameDescriptionLabel;
    [Export] private TextureRect _miniGameTextureRect;
    
    public MiniGameData MiniGameData { get; set; }
    public PlayerData[] PlayerDatas { get; set; }
    
    private MiniGameLoadingScreenPlayerSlot[] _playerSlots;

    public void UpdateMiniGameUI()
    {
        this._miniGameNameLabel.Text = this.MiniGameData.Name;
        this._miniGameDescriptionLabel.Text = this.MiniGameData.Description;
        this._miniGameTextureRect.Texture = this.MiniGameData.Texture2D;
    }

    public void UpdatePlayerDatasUI()
    {
        foreach (Node child in this._miniGameLoadingScreenPlayerSlotContainer.GetChildren())
        {
            child.QueueFree();
        }
        foreach (PlayerData playerData in this.PlayerDatas)
        {
            MiniGameLoadingScreenPlayerSlot playerSlot = this._miniGameLoadingScreenPlayerSlotScene.Instantiate<MiniGameLoadingScreenPlayerSlot>();
            playerSlot.PlayerData = playerData;
            this._miniGameLoadingScreenPlayerSlotContainer.AddChild(playerSlot);
        }
        this._playerSlots = this._miniGameLoadingScreenPlayerSlotContainer.GetChildren().Where(c =>  c is MiniGameLoadingScreenPlayerSlot).Cast<MiniGameLoadingScreenPlayerSlot>().ToArray();
    }
}