using Game.Mini_Games;
using Game.Players;
using Godot;
using Godot.Collections;

namespace Game;

[GlobalClass]
public partial class GameResultData : Resource
{
    public Dictionary<PlayerData, Array<PlayerMiniGameResult>> Players = new();
}