using StreetVelocity.Progression;
using UnityEngine;
using UnityEngine.UI;

namespace StreetVelocity.UI
{
    [RequireComponent(typeof(Button))]
    public sealed class UpgradePurchaseButton : MonoBehaviour
    {
        private Button button;
        private Text label;

        private void Awake()
        {
            button = GetComponent<Button>();
            label = GetComponentInChildren<Text>();
            button.onClick.AddListener(Purchase);
            Refresh();
        }

        private void Purchase()
        {
            if (!PlayerCareerSave.TryUpgradeEngine(out _))
            {
                if (label != null) label.text = $"NEED {PlayerCareerSave.NextEngineUpgradeCost:N0} CREDITS";
                return;
            }
            Refresh();
        }

        private void Refresh()
        {
            if (label != null) label.text = $"ENGINE LV {PlayerCareerSave.EngineLevel} → {PlayerCareerSave.EngineLevel + 1} · {PlayerCareerSave.NextEngineUpgradeCost:N0} CR";
        }

        private void OnDestroy()
        {
            if (button != null) button.onClick.RemoveListener(Purchase);
        }
    }
}
