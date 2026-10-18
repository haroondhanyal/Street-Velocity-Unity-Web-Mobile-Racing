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

        public static int Credits => PlayerPrefs.GetInt(CreditsKey, 125000);
        public static int Experience => PlayerPrefs.GetInt(ExperienceKey, 0);
        public static int Level => PlayerPrefs.GetInt(LevelKey, 1);
        public static int Wins => PlayerPrefs.GetInt(WinsKey, 0);
        public static float BestTime => PlayerPrefs.GetFloat(BestTimeKey, 0f);
        public static int EngineLevel => PlayerPrefs.GetInt(EngineLevelKey, 1);
        public static int NextEngineUpgradeCost => 15000 * EngineLevel;
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
            newLevel = EngineLevel;
            var cost = NextEngineUpgradeCost;
            if (Credits < cost) return false;
            AddCredits(-cost);
            newLevel = EngineLevel + 1;
            PlayerPrefs.SetInt(EngineLevelKey, newLevel);
            PlayerPrefs.Save();
            return true;
        }

        private static void AddCredits(int amount)
        {
            var balance = Credits;
            var next = amount >= 0
                ? (balance > int.MaxValue - amount ? int.MaxValue : balance + amount)
                : Mathf.Max(0, balance + amount);
            PlayerPrefs.SetInt(CreditsKey, next);
        }

        public static void RecordRace(float time, int credits, int experience)
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
            PlayerPrefs.SetInt(WinsKey, Wins + 1);
            var best = BestTime;
            if (best <= 0f || time < best) PlayerPrefs.SetFloat(BestTimeKey, time);
            PlayerPrefs.Save();
        }

        private static int ExperienceForNextLevel(int level) => Mathf.Min(100000, 100 + Mathf.Max(0, level - 1) * 50);
    }
}
