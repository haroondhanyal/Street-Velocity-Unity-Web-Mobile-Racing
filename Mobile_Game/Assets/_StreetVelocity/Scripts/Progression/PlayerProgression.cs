using System;

namespace StreetVelocity.Progression
{
    public sealed class PlayerProgression
    {
        public int Level { get; private set; } = 1;
        public int Experience { get; private set; }
        public event Action<int> LevelChanged;
        public void AddExperience(int amount)
        {
            if (amount <= 0) return;
            Experience = Experience > int.MaxValue - amount ? int.MaxValue : Experience + amount;
            while (Experience >= ExperienceToNextLevel(Level))
            {
                Experience -= ExperienceToNextLevel(Level++);
                LevelChanged?.Invoke(Level);
            }
        }
        private static int ExperienceToNextLevel(int level) => Math.Min(100000, 100 + Math.Max(0, level - 1) * 50);
    }
}
