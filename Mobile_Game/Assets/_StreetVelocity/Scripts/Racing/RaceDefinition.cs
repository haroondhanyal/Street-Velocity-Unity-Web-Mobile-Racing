using UnityEngine;

namespace StreetVelocity.Racing
{
    [CreateAssetMenu(menuName = "Street Velocity/Race Definition", fileName = "RaceDefinition")]
    public sealed class RaceDefinition : ScriptableObject
    {
        [Min(1)] public int laps = 2;
        [Min(1)] public int checkpointCount = 4;
        [Min(0)] public int creditsReward = 250;
        [Min(0)] public int xpReward = 100;
    }
}
