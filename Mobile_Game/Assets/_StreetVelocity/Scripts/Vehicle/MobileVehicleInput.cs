using UnityEngine;
using StreetVelocity.Progression;

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
        public void SetSteering(float value)
        {
            if (PlayerCareerSave.ControlMode == 2) return;
            var direction = PlayerCareerSave.InvertSteering ? -1f : 1f;
            Steering = Mathf.Clamp(value * PlayerCareerSave.ControlSensitivity * direction, -1f, 1f);
        }
        public void SetBrake(bool held) => Brake = held;
        public void SetHandbrake(bool held) => Handbrake = held;
        public void SetNitro(bool held) => Nitro = held;
        public void ReleaseAll()
        {
            Throttle = 0f; Steering = 0f; Brake = false; Handbrake = false; Nitro = false;
        }

        private void Update()
        {
            if (PlayerCareerSave.ControlMode != 2 || !Application.isMobilePlatform) return;
            var direction = PlayerCareerSave.InvertSteering ? -1f : 1f;
            Steering = Mathf.Clamp(Input.acceleration.x * 1.8f * PlayerCareerSave.ControlSensitivity * direction, -1f, 1f);
        }
        private void OnDisable() => ReleaseAll();
    }
}
