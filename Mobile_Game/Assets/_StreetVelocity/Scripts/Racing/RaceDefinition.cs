using UnityEngine;

namespace StreetVelocity.Racing
{
    [CreateAssetMenu(menuName = "Street Velocity/Race Definition", fileName = "RaceDefinition")]
    public sealed class RaceDefinition : ScriptableObject
    {
        [Min(1)] public int laps = 1;
        [Min(1)] public int checkpointCount = 4;
        [Min(0f)] public float countdownSeconds = 3f;
        [Min(0)] public int creditsReward = 8000;
        [Min(0)] public int xpReward = 1200;
    }
}
