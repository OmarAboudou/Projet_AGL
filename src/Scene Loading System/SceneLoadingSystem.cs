using System;
using Common.Composition_System.Inject_Attributes;
using Godot;

namespace Scene_Loading_System;

public partial class SceneLoadingSystem : Node
{
    [Export, InjectChild]
    private TransitionScreen _transitionScreen;
    
    [Export]
    private Node _currentLoadedSceneRoot = null;
    
    public Node LoadScene(PackedScene scene)
    {
        Node instance = scene.Instantiate();
        if (instance == null)
        {
            GD.PrintErr("Scene loading failed");
            return null;
        }
        
        this._transitionScreen.TransitionScreenShown += TransitionScreenOnTransitionScreenShown;
        this._transitionScreen.ShowTransitionScreen();
        
        void TransitionScreenOnTransitionScreenShown()
        {
            this._transitionScreen.TransitionScreenShown -= TransitionScreenOnTransitionScreenShown;
            
            instance.Ready += InstanceOnReady;

            void InstanceOnReady()
            {
                instance.Ready -= InstanceOnReady;
                this._transitionScreen.HideTransitionScreen();
                this.EmitSignalSceneLoaded(instance);
            }
            this.AddChild(instance);
        }
        
        return instance;
    }


    [Signal]
    public delegate void SceneLoadedEventHandler(Node sceneRoot);
}