using Common.Composition_System.Inject_Attributes;
using Godot;
using Main_Menu;

namespace Splash_Screen;

public partial class SplashScreen : Control
{
    [Export] private PackedScene _mainMenuScene;
    [Export, InjectAncestor] private MainMenuSystem _mainMenuSystem;

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (Input.IsAnythingPressed())
        {
            this._mainMenuSystem.StackMenuScene(this._mainMenuScene);
        }
    }
}