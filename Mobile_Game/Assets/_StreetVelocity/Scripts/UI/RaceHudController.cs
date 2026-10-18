using StreetVelocity.Progression;
using StreetVelocity.Racing;
using StreetVelocity.Vehicle;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace StreetVelocity.UI
{
    /// <summary>Race telemetry, countdown, checkpoint progress, and post race actions.</summary>
    public sealed class RaceHudController : MonoBehaviour
    {
        [SerializeField] private VehicleController vehicle;
        [SerializeField] private RaceManager raceManager;
        [SerializeField] private Text speedText;
        [SerializeField] private Text gearText;
        [SerializeField] private Text timerText;
        [SerializeField] private Text lapText;
        [SerializeField] private Text checkpointText;
        [SerializeField] private Text countdownText;
        [SerializeField] private Text resultTitle;
        [SerializeField] private Text resultSummary;
        [SerializeField] private Image nitroFill;
        [SerializeField] private GameObject finishPanel;
        [SerializeField] private GameObject drivingControlsRoot;
        [SerializeField] private Button retryButton;
        [SerializeField] private Button garageButton;
        private float countdownClearTimer;
        private bool rewardGranted;

        private void Awake()
        {
            if (raceManager != null)
            {
                raceManager.ProgressChanged += UpdateProgress;
                raceManager.CountdownChanged += UpdateCountdown;
                raceManager.Finished += ShowResults;
                UpdateProgress(raceManager.CurrentLap, raceManager.NextCheckpoint);
            }
            if (finishPanel != null) finishPanel.SetActive(false);
            if (retryButton != null) retryButton.onClick.AddListener(RetryRace);
            if (garageButton != null) garageButton.onClick.AddListener(ReturnToGarage);
        }

        private void Update()
        {
            if (vehicle != null)
            {
                if (speedText != null) speedText.text = Mathf.RoundToInt(vehicle.SpeedKmh).ToString("000");
                if (gearText != null) gearText.text = Mathf.Clamp(1 + Mathf.FloorToInt(vehicle.SpeedKmh / 55f), 1, 6).ToString();
                if (nitroFill != null) nitroFill.fillAmount = vehicle.Nitro01;
            }
            if (raceManager != null && timerText != null)
            {
                var seconds = raceManager.ElapsedSeconds;
                timerText.text = $"{Mathf.FloorToInt(seconds / 60f):00}:{Mathf.FloorToInt(seconds % 60f):00}.{Mathf.FloorToInt((seconds % 1f) * 1000f):000}";
            }
            if (countdownClearTimer > 0f)
            {
                countdownClearTimer -= Time.deltaTime;
                if (countdownClearTimer <= 0f && countdownText != null) countdownText.gameObject.SetActive(false);
            }
        }

        private void UpdateCountdown(float seconds)
        {
            if (countdownText == null) return;
            countdownText.gameObject.SetActive(true);
            countdownText.text = seconds > 0f ? Mathf.CeilToInt(seconds).ToString() : "GO!";
            if (seconds <= 0f) countdownClearTimer = 0.8f;
        }

        private void UpdateProgress(int lap, int nextCheckpoint)
        {
            if (raceManager == null || raceManager.Definition == null) return;
            if (lapText != null) lapText.text = $"LAP {lap} / {raceManager.Definition.laps}";
            if (checkpointText != null) checkpointText.text = $"CHECKPOINT {nextCheckpoint + 1} / {raceManager.CheckpointsPerLap}";
        }

        private void ShowResults(float time)
        {
            if (rewardGranted || raceManager == null || raceManager.Definition == null) return;
            rewardGranted = true;
            PlayerCareerSave.RecordRace(time, raceManager.Definition.creditsReward, raceManager.Definition.xpReward);
            if (resultTitle != null) resultTitle.text = "RACE COMPLETE · 1ST PLACE";
            if (resultSummary != null)
            {
                var best = PlayerCareerSave.BestTime;
                resultSummary.text = $"TIME  {FormatTime(time)}\nBEST  {FormatTime(best)}\nREWARD  {raceManager.Definition.creditsReward:N0} CREDITS  ·  {raceManager.Definition.xpReward:N0} XP\nLEVEL  {PlayerCareerSave.Level}  ·  WALLET  {PlayerCareerSave.Credits:N0}";
            }
            if (finishPanel != null) finishPanel.SetActive(true);
            if (finishPanel != null) finishPanel.transform.SetAsLastSibling();
            if (drivingControlsRoot != null) drivingControlsRoot.SetActive(false);
        }

        private static string FormatTime(float seconds) => $"{Mathf.FloorToInt(seconds / 60f):00}:{Mathf.FloorToInt(seconds % 60f):00}.{Mathf.FloorToInt((seconds % 1f) * 1000f):000}";
        private static void RetryRace() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        private static void ReturnToGarage() => SceneManager.LoadScene("01_MainMenu");

        private void OnDestroy()
        {
            if (raceManager != null)
            {
                raceManager.ProgressChanged -= UpdateProgress;
                raceManager.CountdownChanged -= UpdateCountdown;
                raceManager.Finished -= ShowResults;
            }
            if (retryButton != null) retryButton.onClick.RemoveListener(RetryRace);
            if (garageButton != null) garageButton.onClick.RemoveListener(ReturnToGarage);
        }
    }
}
