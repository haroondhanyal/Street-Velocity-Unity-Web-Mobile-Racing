using StreetVelocity.Progression;
using UnityEngine;
using UnityEngine.UI;

namespace StreetVelocity.UI
{
    [RequireComponent(typeof(Button))]
    public sealed class DailyRewardClaimButton : MonoBehaviour
    {
        private Button button;
        private Text label;

        private void Awake()
        {
            button = GetComponent<Button>();
            label = GetComponentInChildren<Text>();
            button.onClick.AddListener(Claim);
            Refresh();
        }

        private void Claim()
        {
            if (PlayerCareerSave.TryClaimDailyReward(out var credits))
            {
                if (label != null) label.text = $"CLAIMED · {credits:N0} CR";
                button.interactable = false;
            }
            else Refresh();
        }

        private void Refresh()
        {
            var claimed = PlayerCareerSave.DailyRewardClaimedToday;
            if (label != null) label.text = claimed ? "CLAIMED TODAY" : "CLAIM DAILY REWARD";
            if (button != null) button.interactable = !claimed;
        }

        private void OnDestroy()
        {
            if (button != null) button.onClick.RemoveListener(Claim);
        }
    }
}
