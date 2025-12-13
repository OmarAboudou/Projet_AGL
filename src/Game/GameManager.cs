using System.Collections.Generic;
using Game.Loading_Screen;
using Game.Mini_Games;
using Game.Players;
using Godot;

namespace Game;

public partial class GameManager : Node
{
    [Export] private PlayerData[] _players;
    [Export] private MiniGameData[] _miniGames;
    [Export] private MiniGameLoadingScreen _loadingScreen;

    [Export] private GameResultData _gameResultData = new();
    private MiniGame _currentMiniGame;
    private int _currentMiniGameIndex = 0;
    
    public void Initialize(PlayerData[] players, MiniGameData[] miniGames)
    {
        this._players = players;
        this._miniGames = miniGames;
    }

    public override void _Ready()
    {
        base._Ready();
        foreach (PlayerData playerData in this._players)
        {
            this._gameResultData.Players[playerData] = new();
        }
        this.StartNextMiniGame();
    }

    public void StartNextMiniGame()
    {
        if (this._currentMiniGameIndex >= this._miniGames.Length)
        {
            // TODO : End of game screen.
            this.GetTree().Quit();
            return;
        }
        MiniGameData miniGameData = this._miniGames[this._currentMiniGameIndex];
        MiniGame miniGame = miniGameData.Scene.Instantiate<MiniGame>();
        this._loadingScreen.MiniGameData = this._miniGames[this._currentMiniGameIndex];
        this._loadingScreen.PlayerDatas = this._players;
        this._loadingScreen.UpdateMiniGameUI();
        this._loadingScreen.UpdatePlayerDatasUI();
        this._loadingScreen.LoadingScreenShown += LoadingScreenOnLoadingScreenShown;
        void LoadingScreenOnLoadingScreenShown()
        {
            this._loadingScreen.LoadingScreenShown -= LoadingScreenOnLoadingScreenShown;
            miniGame.SetPlayerData(this._players);
            miniGame.Ready += MiniGameOnReady;
            void MiniGameOnReady()
            {
                miniGame.Ready -= MiniGameOnReady;
                miniGame.MiniGameEnded += MiniGameOnMiniGameEnded;

                void MiniGameOnMiniGameEnded(Godot.Collections.Dictionary<PlayerData, PlayerMiniGameResult> results)
                {
                    miniGame.MiniGameEnded -= MiniGameOnMiniGameEnded;
                    foreach (KeyValuePair<PlayerData, PlayerMiniGameResult> result in results)
                    {
                        this._gameResultData.Players[result.Key].Add(result.Value);
                        
                    }

                    this._currentMiniGameIndex++;
                    this.StartNextMiniGame();
                }

                this._loadingScreen.HideLoadingScreen();
            }
            this.AddChild(miniGame);
        }
        this._loadingScreen.ShowLoadingScreen();
    }
    
}