using System.Threading.Tasks;
using UnityEngine;

namespace StreetVelocity.UI
{
    /// <summary>Reusable visibility behavior for CanvasGroup-based screens.</summary>
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UIScreen : MonoBehaviour, IUIScreen
    {
        [SerializeField] private string screenId;
        private CanvasGroup canvasGroup;
        public string ScreenId => screenId;

        protected virtual void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            SetVisible(false);
        }

        public virtual Task ShowAsync() { SetVisible(true); return Task.CompletedTask; }
        public virtual Task HideAsync() { SetVisible(false); return Task.CompletedTask; }

        private void SetVisible(bool visible)
        {
            if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = visible ? 1f : 0f;
            canvasGroup.interactable = visible;
            canvasGroup.blocksRaycasts = visible;
        }
    }
}
