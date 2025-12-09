using Godot;

namespace Game;

public partial class GameManager : Node
{
    private GameConfigurationData _configurationData;    
    
    public void Initialize(GameConfigurationData config)
    {
        this._configurationData = config;
    }
}