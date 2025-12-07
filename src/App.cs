using Godot;
using Loading_Screen;

public partial class App : Node
{
    [Export] 
    private PackedScene _initialScene;
    
    [Export] 
    private LoadingScreen _loadingScreen;
    
    private Node _rootOfCurrentScene;
    private static App This;

    public override void _Ready()
    {
        base._Ready();
        if (This != null)
        {
            this.QueueFree();
            GD.Print($"Multiple instances of {nameof(App)} are instantiated. Only the first one is kept.");
            return;
        }
        This = this;
        this.SetupInitialScene();
    }

    public static T ChangeScene<T>(PackedScene newScene) where T : class
    {
        T instance = newScene.Instantiate<T>();
        This._loadingScreen.LoadingScreenShown += LoadingScreenOnLoadingScreenShown;
        void LoadingScreenOnLoadingScreenShown()
        {
            This._loadingScreen.LoadingScreenShown -= LoadingScreenOnLoadingScreenShown;
            This._rootOfCurrentScene = (Node)(object)instance;
            This._rootOfCurrentScene.Ready += RootOfCurrentSceneOnReady;

            void RootOfCurrentSceneOnReady()
            {
                This._rootOfCurrentScene.Ready -= RootOfCurrentSceneOnReady;
                This._loadingScreen.HideLoadingScreen();
            }

            This.AddChild(This._rootOfCurrentScene);
        }
        This._loadingScreen.ShowLoadingScreen();
        return instance;
    }

    private void SetupInitialScene()
    {
        Node instance = this._initialScene.Instantiate();
        this._rootOfCurrentScene = instance;
        this._rootOfCurrentScene.Ready += RootOfCurrentSceneOnReady;
        this.AddChild(instance); 
        void RootOfCurrentSceneOnReady()
        {
            this._rootOfCurrentScene.Ready -= RootOfCurrentSceneOnReady;
            this._loadingScreen.HideLoadingScreen();
        }
        
    }
}
