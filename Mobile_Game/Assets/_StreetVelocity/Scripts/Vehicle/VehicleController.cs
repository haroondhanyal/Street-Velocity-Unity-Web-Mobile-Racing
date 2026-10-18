using StreetVelocity.Progression;
using UnityEngine;

namespace StreetVelocity.Vehicle
{
    /// <summary>Arcade vehicle physics with throttle, braking, drift and a rechargeable nitro meter.</summary>
    [RequireComponent(typeof(Rigidbody))]
    public sealed class VehicleController : MonoBehaviour
    {
        [SerializeField] private VehicleConfig config;
        [SerializeField] private MonoBehaviour inputSource;
        private Rigidbody body;
        private IVehicleInput input;
        private float nitroSeconds;
        private bool drivingEnabled = true;

        public float SpeedKmh => body == null ? 0f : body.velocity.magnitude * 3.6f;
        public float Nitro01 => config == null || config.nitroCapacitySeconds <= 0f ? 0f : nitroSeconds / config.nitroCapacitySeconds;

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
            input = inputSource as IVehicleInput;
            if (config != null) nitroSeconds = config.nitroCapacitySeconds;
            body.interpolation = RigidbodyInterpolation.Interpolate;
            body.centerOfMass = new Vector3(0f, -0.35f, 0f);
            if (config == null) Debug.LogError("VehicleConfig is missing.", this);
            if (input == null) Debug.LogError("Input source must implement IVehicleInput.", this);
        }

        public void SetDrivingEnabled(bool enabled)
        {
            drivingEnabled = enabled;
            if (!enabled && body != null)
            {
                body.velocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;
            }
        }

        private void FixedUpdate()
        {
            if (!drivingEnabled || config == null || input == null) return;
            var forwardSpeed = Vector3.Dot(body.velocity, transform.forward);
            var throttle = input.Throttle;
            var engineScale = 1f + Mathf.Max(0, PlayerCareerSave.EngineLevel - 1) * 0.035f;
            var boosting = input.Nitro && nitroSeconds > 0f;
            var speedLimit = config.maxSpeed * engineScale * (boosting ? 1.16f : 1f);
            if (throttle > 0f && forwardSpeed < speedLimit)
                body.AddForce(transform.forward * (throttle * config.acceleration * engineScale), ForceMode.Acceleration);
            if (input.Brake)
                body.AddForce(-transform.forward * (forwardSpeed > 1f ? config.braking : config.reverseAcceleration), ForceMode.Acceleration);

            if (boosting)
            {
                body.AddForce(transform.forward * config.nitroAcceleration, ForceMode.Acceleration);
                nitroSeconds = Mathf.Max(0f, nitroSeconds - Time.fixedDeltaTime);
            }
            else
            {
                nitroSeconds = Mathf.Min(config.nitroCapacitySeconds, nitroSeconds + Time.fixedDeltaTime * 0.16f);
            }

            var speedFactor = Mathf.Clamp01(body.velocity.magnitude / 4f);
            var turn = input.Steering * config.steeringDegreesPerSecond * speedFactor * Time.fixedDeltaTime;
            if (input.Handbrake) turn *= 1.55f;
            body.MoveRotation(body.rotation * Quaternion.Euler(0f, turn, 0f));
            var lateral = transform.right * Vector3.Dot(body.velocity, transform.right);
            var grip = input.Handbrake ? config.handbrakeGrip : config.lateralGrip;
            body.AddForce(-lateral * grip, ForceMode.Acceleration);
        }
    }
}
