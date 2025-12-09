using Godot;

namespace Game.Mini_Games;

[GlobalClass]
public partial class MiniGameData : Resource
{
    [Export] public string Name { get; private set; }
    [Export(PropertyHint.MultilineText)] public string Description { get; private set; }
    [Export] public PackedScene Scene { get; private set; }
    [Export] public Texture2D Texture2D { get; private set; }
}