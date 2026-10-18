using UnityEngine;

namespace StreetVelocity.Vehicle
{
    [CreateAssetMenu(menuName = "Street Velocity/Vehicle Config", fileName = "VehicleConfig")]
    public sealed class VehicleConfig : ScriptableObject
    {
        [Min(0)] public float acceleration = 24f;
        [Min(0)] public float reverseAcceleration = 12f;
        [Min(0)] public float braking = 32f;
        [Min(0)] public float maxSpeed = 55f;
        [Min(0)] public float steeringDegreesPerSecond = 95f;
        [Range(0.1f, 2f)] public float lateralGrip = 1f;
        [Min(0)] public float nitroAcceleration = 28f;
        [Min(0.2f)] public float nitroCapacitySeconds = 3f;
        [Range(0.05f, 1f)] public float handbrakeGrip = 0.18f;
    }
}
