using StreetVelocity.Progression;
using UnityEngine;
using UnityEngine.UI;

namespace StreetVelocity.UI
{
    public sealed class CareerOverviewController : MonoBehaviour
    {
        [SerializeField] private Text careerSummary;
        [SerializeField] private Text dailySummary;

        private void Awake() => Refresh();
        private void Start() => Refresh();

        public void Refresh()
        {
            if (careerSummary != null)
                careerSummary.text = $"LEVEL {PlayerCareerSave.Level}  ·  {PlayerCareerSave.Wins} WINS  ·  {PlayerCareerSave.Credits:N0} CR";
            if (dailySummary != null)
                dailySummary.text = $"{PlayerCareerSave.DailyStreak} DAY STREAK  ·  {(PlayerCareerSave.DailyRewardClaimedToday ? "CLAIMED TODAY" : "READY TO CLAIM")}";
        }
    }
}
