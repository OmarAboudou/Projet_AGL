using Godot;

namespace Loading_Screen;

public partial class LoadingScreen : Control
{
    [Export] private AnimationPlayer _fadeAnimationPlayer;
    [Export] private StringName _fadeToTransparentAnimationName;
    [Export] private StringName _fadeToBlackAnimationName;

    [Signal]
    public delegate void LoadingScreenShownEventHandler();
    
    [Signal]
    public delegate void LoadingScreenHiddenEventHandler();

    public override void _Ready()
    {
        base._Ready();
        this._fadeAnimationPlayer.AnimationFinished += this.FadeAnimationPlayerOnAnimationFinished;
    }

    public void ShowLoadingScreen()
    {
        this._fadeAnimationPlayer.Queue(this._fadeToBlackAnimationName);
    }

    public void HideLoadingScreen()
    {
        this._fadeAnimationPlayer.Queue(this._fadeToTransparentAnimationName);
    }

    private void FadeAnimationPlayerOnAnimationFinished(StringName animName)
    {
        if (animName == this._fadeToTransparentAnimationName)
        {
            this.EmitSignalLoadingScreenHidden();
        }
        else if (animName == this._fadeToBlackAnimationName)
        {
            this.EmitSignalLoadingScreenShown();
        }
    }
}