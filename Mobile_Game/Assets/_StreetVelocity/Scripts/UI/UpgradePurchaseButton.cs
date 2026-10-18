using StreetVelocity.Progression;
using UnityEngine;
using UnityEngine.UI;

namespace StreetVelocity.UI
{
    [RequireComponent(typeof(Button))]
    public sealed class UpgradePurchaseButton : MonoBehaviour
    {
        [SerializeField, Range(0, 7)] private int partIndex;
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
            if (!PlayerCareerSave.TryUpgradePart(partIndex, out _))
            {
                if (label != null) label.text = PlayerCareerSave.PartLevel(partIndex) >= 10 ? $"{PlayerCareerSave.PartName(partIndex)} MAX LEVEL" : $"NEED {PlayerCareerSave.PartUpgradeCost(partIndex):N0} CREDITS";
                return;
            }
            Refresh();
        }

        private void Refresh()
        {
            if (label != null) label.text = $"{PlayerCareerSave.PartName(partIndex)} LV {PlayerCareerSave.PartLevel(partIndex)} → {PlayerCareerSave.PartLevel(partIndex) + 1} · {PlayerCareerSave.PartUpgradeCost(partIndex):N0} CR";
        }

        private void OnDestroy()
        {
            if (button != null) button.onClick.RemoveListener(Purchase);
        }
    }
}
