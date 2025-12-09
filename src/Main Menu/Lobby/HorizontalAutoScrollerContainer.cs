using Godot;

namespace Main_Menu.Lobby;

[Tool, GlobalClass]
public partial class HorizontalAutoScrollerContainer : ScrollContainer
{
    private Control _controlToScroll;

    [Export]
    private float ScrollDuration
    {
        get => this._scrollDuration;
        set
        {
            this._scrollDuration = value;
            this.RecomputeTween();
        }
    }

    [Export]
    private float PausesDuration
    {
        get => this._pausesDuration;
        set
        {
            this._pausesDuration = value;
            this.RecomputeTween();
        }
    }

    private Tween _scrollTween;
    private float _scrollDuration = 2;
    private float _pausesDuration = 2;

    public override void _Ready()
    {
        base._Ready();
        this._controlToScroll = this.GetChild<Control>(0);
        }

    private void ScrollRatio(float ratio)
    {
        float horizontalSizeDelta = this._controlToScroll.Size.X - this.Size.X;
        this.ScrollHorizontal = Mathf.RoundToInt(horizontalSizeDelta * ratio);
    }

    private void RecomputeTween()
    {
        this._scrollTween?.Kill();
        this._scrollTween = this.CreateTween();
        this._scrollTween.SetLoops();
        this._scrollTween.TweenMethod(Callable.From<float>(this.ScrollRatio), 0f, 1f, this.ScrollDuration);
        this._scrollTween.TweenInterval(this.PausesDuration);
        this._scrollTween.TweenMethod(Callable.From<float>(this.ScrollRatio), 1f, 0f, this.ScrollDuration);
        this._scrollTween.TweenInterval(this.PausesDuration);
    }
    
    
}