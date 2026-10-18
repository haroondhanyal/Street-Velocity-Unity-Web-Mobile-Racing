using UnityEngine;

namespace StreetVelocity.Vehicle
{
    /// <summary>Wire UI pointer down/up events to the public setters.</summary>
    public sealed class MobileVehicleInput : MonoBehaviour, IVehicleInput
    {
        public float Throttle { get; private set; }
        public float Steering { get; private set; }
        public bool Brake { get; private set; }
        public bool Handbrake { get; private set; }
        public bool Nitro { get; private set; }
        public void SetThrottle(bool held) => Throttle = held ? 1f : 0f;
        public void SetSteering(float value) => Steering = Mathf.Clamp(value, -1f, 1f);
        public void SetBrake(bool held) => Brake = held;
        public void SetHandbrake(bool held) => Handbrake = held;
        public void SetNitro(bool held) => Nitro = held;
        public void ReleaseAll()
        {
            Throttle = 0f; Steering = 0f; Brake = false; Handbrake = false; Nitro = false;
        }
        private void OnDisable() => ReleaseAll();
    }
}
