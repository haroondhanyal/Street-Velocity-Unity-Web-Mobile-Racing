using StreetVelocity.Progression;
using UnityEngine;
using UnityEngine.UI;

namespace StreetVelocity.UI
{
    public enum ControlPreferenceAction { ToggleInvert, CycleSensitivity }

    [RequireComponent(typeof(Button))]
    public sealed class ControlPreferenceButton : MonoBehaviour
    {
        [SerializeField] private ControlPreferenceAction action;
        private Button button;
        private Text label;

        private void Awake()
        {
            button = GetComponent<Button>();
            label = GetComponentInChildren<Text>();
            button.onClick.AddListener(Apply);
            Refresh();
        }

        private void Apply()
        {
            if (action == ControlPreferenceAction.ToggleInvert)
                PlayerCareerSave.SetInvertSteering(!PlayerCareerSave.InvertSteering);
            else
                PlayerCareerSave.SetControlSensitivity(PlayerCareerSave.ControlSensitivity >= 1.8f ? 10 : Mathf.RoundToInt(PlayerCareerSave.ControlSensitivity * 50f) + 10);
            Refresh();
        }

        private void Refresh()
        {
            if (label == null) return;
            label.text = action == ControlPreferenceAction.ToggleInvert
                ? $"INVERT STEERING  ·  {(PlayerCareerSave.InvertSteering ? "ON" : "OFF")}" 
                : $"SENSITIVITY  ·  {Mathf.RoundToInt(PlayerCareerSave.ControlSensitivity * 50f)}%  ·  TAP TO CYCLE";
        }

        private void OnDestroy()
        {
            if (button != null) button.onClick.RemoveListener(Apply);
        }
    }
}
