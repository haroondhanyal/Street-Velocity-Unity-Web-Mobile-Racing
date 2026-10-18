using System;
using UnityEngine;

namespace StreetVelocity.Progression
{
    /// <summary>Small local save store for the Phase 3 race rewards and driver level.</summary>
    public static class PlayerCareerSave
    {
        private const string CreditsKey = "StreetVelocity.Credits";
        private const string ExperienceKey = "StreetVelocity.Experience";
        private const string LevelKey = "StreetVelocity.Level";
        private const string WinsKey = "StreetVelocity.Wins";
        private const string BestTimeKey = "StreetVelocity.BestTime";
        private const string EngineLevelKey = "StreetVelocity.EngineLevel";
        private const string DailyClaimDateKey = "StreetVelocity.DailyClaimDate";
        private const string DailyStreakKey = "StreetVelocity.DailyStreak";
        private const string SelectedCarKey = "StreetVelocity.SelectedCar";
        private const string SelectedEventKey = "StreetVelocity.SelectedEvent";
        private const string SelectedPaintKey = "StreetVelocity.SelectedPaint";
        private const string ControlModeKey = "StreetVelocity.ControlMode";
        private const string ControlSensitivityKey = "StreetVelocity.ControlSensitivity";
        private const string InvertSteeringKey = "StreetVelocity.InvertSteering";
        private const string HighestSpeedKey = "StreetVelocity.HighestSpeed";
        private const string BestDriftKey = "StreetVelocity.BestDrift";
        private const string PoliceEscapesKey = "StreetVelocity.PoliceEscapes";
        private const string DriftPointsKey = "StreetVelocity.DriftPoints";
        private const string GraphicsQualityKey = "StreetVelocity.GraphicsQuality";
        private const string WeatherKey = "StreetVelocity.Weather";

        public static int Credits => PlayerPrefs.GetInt(CreditsKey, 125000);
        public static int Experience => PlayerPrefs.GetInt(ExperienceKey, 0);
        public static int Level => PlayerPrefs.GetInt(LevelKey, 1);
        public static int Wins => PlayerPrefs.GetInt(WinsKey, 0);
        public static int DailyStreak => PlayerPrefs.GetInt(DailyStreakKey, 0);
        public static float HighestSpeed => PlayerPrefs.GetFloat(HighestSpeedKey, 0f);
        public static int BestDrift => PlayerPrefs.GetInt(BestDriftKey, 0);
        public static int PoliceEscapes => PlayerPrefs.GetInt(PoliceEscapesKey, 0);
        public static int DriftPoints => PlayerPrefs.GetInt(DriftPointsKey, 0);
        public static int GraphicsQuality => Mathf.Clamp(PlayerPrefs.GetInt(GraphicsQualityKey, 2), 0, 2);
        public static int Weather => Mathf.Clamp(PlayerPrefs.GetInt(WeatherKey, 1), 0, 3);
        public static float BestTime => PlayerPrefs.GetFloat(BestTimeKey, 0f);
        public static int EngineLevel => PlayerPrefs.GetInt(EngineLevelKeyForCar(SelectedCar), PlayerPrefs.GetInt(EngineLevelKey, 1));
        public static int SelectedCar => Mathf.Clamp(PlayerPrefs.GetInt(SelectedCarKey, 0), 0, 4);
        public static int SelectedEvent => Mathf.Clamp(PlayerPrefs.GetInt(SelectedEventKey, 0), 0, 4);
        public static string SelectedPaint => PlayerPrefs.GetString(SelectedPaintKey, "#D91E45");
        public static int ControlMode => Mathf.Clamp(PlayerPrefs.GetInt(ControlModeKey, 0), 0, 2);
        public static float ControlSensitivity => Mathf.Clamp(PlayerPrefs.GetInt(ControlSensitivityKey, 50), 10, 100) / 50f;
        public static bool InvertSteering => PlayerPrefs.GetInt(InvertSteeringKey, 0) == 1;
        public static int NextEngineUpgradeCost => 15000 * EngineLevel;
        public static int PartLevel(int partIndex)
        {
            if (partIndex == 0) return EngineLevel;
            return PlayerPrefs.GetInt(PartLevelKey(partIndex), 1);
        }
        public static int PartUpgradeCost(int partIndex) => 10000 * PartLevel(partIndex);
        public static string PartName(int partIndex) => new[] { "ENGINE", "TURBO", "ECU", "TRANSMISSION", "TIRES", "BRAKES", "SUSPENSION", "NITRO" }[Mathf.Clamp(partIndex, 0, 7)];
        public static bool DailyRewardClaimedToday => PlayerPrefs.GetString(DailyClaimDateKey, string.Empty) == DateTime.Now.ToString("yyyy-MM-dd");

        public static bool TryClaimDailyReward(out int reward)
        {
            reward = 0;
            if (DailyRewardClaimedToday) return false;

            var today = DateTime.Now.Date;
            var previous = PlayerPrefs.GetString(DailyClaimDateKey, string.Empty);
            var yesterday = today.AddDays(-1).ToString("yyyy-MM-dd");
            var streak = previous == yesterday ? PlayerPrefs.GetInt(DailyStreakKey, 0) + 1 : 1;
            reward = Mathf.Min(5000, 1000 + (streak - 1) * 500);
            PlayerPrefs.SetString(DailyClaimDateKey, today.ToString("yyyy-MM-dd"));
            PlayerPrefs.SetInt(DailyStreakKey, streak);
            AddCredits(reward);
            PlayerPrefs.Save();
            return true;
        }

        public static bool TryUpgradeEngine(out int newLevel)
        {
            return TryUpgradePart(0, out newLevel);
        }

        public static bool TryUpgradePart(int partIndex, out int newLevel)
        {
            partIndex = Mathf.Clamp(partIndex, 0, 7);
            var level = PartLevel(partIndex);
            newLevel = level;
            if (level >= 10 || Credits < PartUpgradeCost(partIndex)) return false;
            AddCredits(-PartUpgradeCost(partIndex));
            newLevel = level + 1;
            if (partIndex == 0) PlayerPrefs.SetInt(EngineLevelKeyForCar(SelectedCar), newLevel);
            else PlayerPrefs.SetInt(PartLevelKey(partIndex), newLevel);
            PlayerPrefs.Save();
            return true;
        }

        private static string PartLevelKey(int partIndex) => $"StreetVelocity.Car{SelectedCar}.Part{partIndex}Level";
        private static string EngineLevelKeyForCar(int carIndex) => $"StreetVelocity.Car{Mathf.Clamp(carIndex, 0, 4)}.EngineLevel";

        public static void SelectCar(int index)
        {
            PlayerPrefs.SetInt(SelectedCarKey, Mathf.Clamp(index, 0, 4));
            PlayerPrefs.Save();
        }

        public static void SelectEvent(int index)
        {
            PlayerPrefs.SetInt(SelectedEventKey, Mathf.Clamp(index, 0, 4));
            PlayerPrefs.Save();
        }

        public static void SelectPaint(string hex)
        {
            if (ColorUtility.TryParseHtmlString(hex, out _))
            {
                PlayerPrefs.SetString(SelectedPaintKey, hex);
                PlayerPrefs.Save();
            }
        }

        public static void SelectControlMode(int mode)
        {
            PlayerPrefs.SetInt(ControlModeKey, Mathf.Clamp(mode, 0, 2));
            PlayerPrefs.Save();
        }

        public static void SetControlSensitivity(int value)
        {
            PlayerPrefs.SetInt(ControlSensitivityKey, Mathf.Clamp(value, 10, 100));
            PlayerPrefs.Save();
        }

        public static void SetInvertSteering(bool inverted)
        {
            PlayerPrefs.SetInt(InvertSteeringKey, inverted ? 1 : 0);
            PlayerPrefs.Save();
        }

        public static void SelectGraphicsQuality(int quality)
        {
            var selected = Mathf.Clamp(quality, 0, 2);
            PlayerPrefs.SetInt(GraphicsQualityKey, selected);
            PlayerPrefs.Save();
            if (QualitySettings.names.Length > selected) QualitySettings.SetQualityLevel(selected, true);
        }

        public static void SelectWeather(int weather)
        {
            PlayerPrefs.SetInt(WeatherKey, Mathf.Clamp(weather, 0, 3));
            PlayerPrefs.Save();
        }

        private static void AddCredits(int amount)
        {
            var balance = Credits;
            var next = amount >= 0
                ? (balance > int.MaxValue - amount ? int.MaxValue : balance + amount)
                : Mathf.Max(0, balance + amount);
            PlayerPrefs.SetInt(CreditsKey, next);
        }

        public static void RecordRace(float time, int credits, int experience, bool won = true, float drift = 0f, float speed = 0f, int eventIndex = 0)
        {
            AddCredits(Mathf.Max(0, credits));

            var xp = Experience;
            var level = Level;
            experience = Mathf.Max(0, experience);
            xp = xp > int.MaxValue - experience ? int.MaxValue : xp + experience;
            var threshold = ExperienceForNextLevel(level);
            while (xp >= threshold)
            {
                xp -= threshold;
                level++;
                threshold = ExperienceForNextLevel(level);
            }

            PlayerPrefs.SetInt(ExperienceKey, xp);
            PlayerPrefs.SetInt(LevelKey, level);
            if (won) PlayerPrefs.SetInt(WinsKey, Wins + 1);
            PlayerPrefs.SetInt(BestDriftKey, Mathf.Max(BestDrift, Mathf.FloorToInt(drift)));
            var addedDrift = Mathf.Max(0, Mathf.FloorToInt(drift));
            PlayerPrefs.SetInt(DriftPointsKey, DriftPoints > int.MaxValue - addedDrift ? int.MaxValue : DriftPoints + addedDrift);
            PlayerPrefs.SetFloat(HighestSpeedKey, Mathf.Max(HighestSpeed, speed));
            if (eventIndex == 4 && won) PlayerPrefs.SetInt(PoliceEscapesKey, PoliceEscapes + 1);
            var best = BestTime;
            if (best <= 0f || time < best) PlayerPrefs.SetFloat(BestTimeKey, time);
            PlayerPrefs.Save();
        }

        private static int ExperienceForNextLevel(int level) => Mathf.Min(100000, 100 + Mathf.Max(0, level - 1) * 50);
    }
}
