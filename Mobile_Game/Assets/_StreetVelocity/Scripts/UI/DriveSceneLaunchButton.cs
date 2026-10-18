using StreetVelocity.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace StreetVelocity.UI
{
    [RequireComponent(typeof(Button))]
    public sealed class DriveSceneLaunchButton : MonoBehaviour
    {
        [SerializeField] private string sceneName = "09_DriveTest";
        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(Launch);
        }

        private async void Launch()
        {
            var bootstrap = FindObjectOfType<GameBootstrap>();
            if (bootstrap != null && bootstrap.SceneLoader != null)
            {
                await bootstrap.SceneLoader.LoadAsync(sceneName);
                return;
            }
            var operation = SceneManager.LoadSceneAsync(sceneName);
            if (operation == null) Debug.LogError($"Could not load driving scene '{sceneName}'. Run the Phase 2 setup first.", this);
        }

        private void OnDestroy()
        {
            if (button != null) button.onClick.RemoveListener(Launch);
        }
    }
}
