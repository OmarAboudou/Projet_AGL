using Game.Mini_Games;
using Game.Players;


namespace Game;

public readonly struct GameConfigurationData(PlayerData[] playerDatas, MiniGameData[] miniGamesDatas)
{
    private readonly PlayerData[] _playerDatas =  playerDatas;
    private readonly MiniGameData[] _miniGameDatas =  miniGamesDatas;
}