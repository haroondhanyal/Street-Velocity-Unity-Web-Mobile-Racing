using System;
using UnityEngine;

namespace StreetVelocity.Core
{
    /// <summary>Composition root. Keep scene-specific references out of feature services.</summary>
    public sealed class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private string mainMenuScene = "01_MainMenu";
        private static GameBootstrap instance;
        public GameStateMachine State { get; private set; }
        public ISceneLoader SceneLoader { get; private set; }
        public GameServiceRegistry Services { get; private set; }

        private void Awake()
        {
            if (instance != null && instance != this) { Destroy(gameObject); return; }
            instance = this;
            DontDestroyOnLoad(gameObject);
            State = new GameStateMachine();
            SceneLoader = new SceneLoader();
            Services = new GameServiceRegistry();
        }

        private async void Start()
        {
            try
            {
                Services.InitializeAll();
                await SceneLoader.LoadAsync(mainMenuScene);
                State.TransitionTo(GameState.MainMenu);
            }
            catch (Exception exception) { Debug.LogError($"Street Velocity startup failed: {exception.Message}", this); }
        }

        private void OnDestroy()
        {
            if (instance != this) return;
            Services?.ShutdownAll();
            instance = null;
        }
    }
}
