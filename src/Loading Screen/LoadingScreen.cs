using Godot;

namespace Loading_Screen;

public partial class LoadingScreen : Control
{
    [Export] private AnimationPlayer _fadeAnimationPlayer;
    [Export] private string _hideLoadingScreenAnimationName = "FadeToTransparentAnimation";
    [Export] private string _showLoadingScreenAnimationName = "FadeToBlackAnimation";

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
        this._fadeAnimationPlayer.Queue(this._showLoadingScreenAnimationName);
    }

    public void HideLoadingScreen()
    {
        this._fadeAnimationPlayer.Queue(this._hideLoadingScreenAnimationName);
    }

    private void FadeAnimationPlayerOnAnimationFinished(StringName animName)
    {
        if (animName == this._hideLoadingScreenAnimationName)
        {
            this.EmitSignalLoadingScreenHidden();
        }
        else if (animName == this._showLoadingScreenAnimationName)
        {
            this.EmitSignalLoadingScreenShown();
        }
    }
}