using StreetVelocity.Vehicle;
using UnityEngine;
using UnityEngine.UI;

namespace StreetVelocity.UI
{
    public sealed class RaceHudController : MonoBehaviour
    {
        [SerializeField] private VehicleController vehicle;
        [SerializeField] private Text speedText;
        [SerializeField] private Text gearText;
        [SerializeField] private Text timerText;
        [SerializeField] private Image nitroFill;
        private float elapsed;

        private void Update()
        {
            if (vehicle == null) return;
            elapsed += Time.deltaTime;
            if (speedText != null) speedText.text = Mathf.RoundToInt(vehicle.SpeedKmh).ToString("000");
            if (gearText != null) gearText.text = Mathf.Clamp(1 + Mathf.FloorToInt(vehicle.SpeedKmh / 55f), 1, 6).ToString();
            if (timerText != null) timerText.text = $"{Mathf.FloorToInt(elapsed / 60f):00}:{Mathf.FloorToInt(elapsed % 60f):00}.{Mathf.FloorToInt((elapsed % 1f) * 1000f):000}";
            if (nitroFill != null) nitroFill.fillAmount = vehicle.Nitro01;
        }
    }
}
