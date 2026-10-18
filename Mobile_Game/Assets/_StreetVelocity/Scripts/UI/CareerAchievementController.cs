using StreetVelocity.Progression;
using UnityEngine;
using UnityEngine.UI;

namespace StreetVelocity.UI
{
    public sealed class CareerAchievementController : MonoBehaviour
    {
        [SerializeField] private Text victoryText;
        [SerializeField] private Text driftText;
        [SerializeField] private Text speedText;

        private void Start() => Refresh();

        public void Refresh()
        {
            if (victoryText != null) victoryText.text = PlayerCareerSave.Wins > 0 ? "UNLOCKED  ✓" : "RACE WINS  0 / 1";
            if (driftText != null) driftText.text = $"TOTAL  {Mathf.Min(PlayerCareerSave.DriftPoints, 50000):N0} / 50,000 PTS";
            if (speedText != null) speedText.text = $"TOP SPEED  {Mathf.Min(PlayerCareerSave.HighestSpeed, 300f):N0} / 300 KM/H";
        }
    }
}
