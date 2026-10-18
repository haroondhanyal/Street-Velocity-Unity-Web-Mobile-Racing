using UnityEngine;
using UnityEngine.UI;

namespace StreetVelocity.UI
{
    [RequireComponent(typeof(Button))]
    public sealed class ScreenNavigationButton : MonoBehaviour
    {
        [SerializeField] private ScreenRouter router;
        [SerializeField] private string targetScreenId;
        public string TargetScreenId => targetScreenId;
        private Button button;
        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(Navigate);
        }
        public void Initialize(ScreenRouter screenRouter, string screenId)
        {
            router = screenRouter;
            targetScreenId = screenId;
        }
        private async void Navigate()
        {
            if (router == null) { Debug.LogError("ScreenRouter reference is missing.", this); return; }
            await router.ShowAsync(targetScreenId);
        }
        private void OnDestroy() { if (button != null) button.onClick.RemoveListener(Navigate); }
    }
}
