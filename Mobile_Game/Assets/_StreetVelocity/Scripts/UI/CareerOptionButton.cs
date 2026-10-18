using StreetVelocity.Progression;
using UnityEngine;
using UnityEngine.UI;

namespace StreetVelocity.UI
{
    public enum CareerOptionType { Car, Event, Paint, Control, Graphics, Weather }

    [RequireComponent(typeof(Button))]
    public sealed class CareerOptionButton : MonoBehaviour
    {
        [SerializeField] private CareerOptionType optionType;
        [SerializeField] private int optionIndex;
        [SerializeField] private string paintHex;
        [SerializeField] private string optionLabel;
        private Button button;
        private Text label;
        private Color baseColor;

        private void Awake()
        {
            button = GetComponent<Button>();
            baseColor = GetComponent<Image>() != null ? GetComponent<Image>().color : Color.white;
            label = GetComponentInChildren<Text>();
            button.onClick.AddListener(Select);
            Refresh();
        }

        private void Select()
        {
            switch (optionType)
            {
                case CareerOptionType.Car: PlayerCareerSave.SelectCar(optionIndex); break;
                case CareerOptionType.Event: PlayerCareerSave.SelectEvent(optionIndex); break;
                case CareerOptionType.Paint: PlayerCareerSave.SelectPaint(paintHex); break;
                case CareerOptionType.Control: PlayerCareerSave.SelectControlMode(optionIndex); break;
                case CareerOptionType.Graphics: PlayerCareerSave.SelectGraphicsQuality(optionIndex); break;
                case CareerOptionType.Weather: PlayerCareerSave.SelectWeather(optionIndex); break;
            }
            foreach (var option in FindObjectsOfType<CareerOptionButton>(true)) option.Refresh();
        }

        public void Refresh()
        {
            var selected = optionType == CareerOptionType.Car && PlayerCareerSave.SelectedCar == optionIndex
                || optionType == CareerOptionType.Event && PlayerCareerSave.SelectedEvent == optionIndex
                || optionType == CareerOptionType.Control && PlayerCareerSave.ControlMode == optionIndex
                || optionType == CareerOptionType.Graphics && PlayerCareerSave.GraphicsQuality == optionIndex
                || optionType == CareerOptionType.Weather && PlayerCareerSave.Weather == optionIndex
                || optionType == CareerOptionType.Paint && PlayerCareerSave.SelectedPaint == paintHex;
            if (label != null) label.text = selected ? $"✓  {optionLabel}  ·  SELECTED" : optionLabel;
            var image = GetComponent<Image>();
            if (image != null) image.color = selected ? new Color(0.04f, 0.58f, 0.78f, 1f) : baseColor;
        }

        private void OnDestroy()
        {
            if (button != null) button.onClick.RemoveListener(Select);
        }
    }
}
