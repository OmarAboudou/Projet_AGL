using Godot;

namespace Custom_Nodes;

[GlobalClass]
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

    public override void _EnterTree()
    {
        base._EnterTree();
        this.VerticalScrollMode = ScrollMode.Disabled;
        this.HorizontalScrollMode = ScrollMode.ShowNever;
        this.ChildOrderChanged += this.InitializeControlToScroll;
        this.RecomputeTween();
    }

    public override void _Ready()
    {
        base._Ready();
        this.InitializeControlToScroll();
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        this.ChildOrderChanged -= this.InitializeControlToScroll;
        this._scrollTween?.Kill();
    }

    private void InitializeControlToScroll()
    {
        this._controlToScroll = this.GetChildOrNull<Control>(0);
    }
    
    private void ScrollRatio(float ratio)
    {
        if(this._controlToScroll == null) return;
        
        float horizontalSizeDelta = this._controlToScroll.Size.X - this.Size.X;
        this.ScrollHorizontal = Mathf.RoundToInt(horizontalSizeDelta * ratio);
    }

    private void RecomputeTween()
    {
        this._scrollTween?.Kill();
        this._scrollTween = this.CreateTween();
        this._scrollTween.SetLoops();
        this._scrollTween.TweenMethod(Callable.From<float>(this.ScrollRatio), 0f, 1f, this.ScrollDuration).SetDelay(this.PausesDuration);
        this._scrollTween.TweenMethod(Callable.From<float>(this.ScrollRatio), 1f, 0f, this.ScrollDuration).SetDelay(this.PausesDuration);
    }
    
    
}