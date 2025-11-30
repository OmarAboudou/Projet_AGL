using Godot;

namespace Scene_Loading_System;

public partial class TransitionScreen : Control
{
    public void ShowTransitionScreen()
    {
        this.Show();
        this.EmitSignalTransitionScreenShown();
    }

    public void HideTransitionScreen()
    {
        this.Hide();
        this.EmitSignalTransitionScreenHidden();
    }

    [Signal]
    public delegate void TransitionScreenShownEventHandler();
    
    [Signal]
    public delegate void TransitionScreenHiddenEventHandler();
}