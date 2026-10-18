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
        public float DriftScore { get; private set; }
        public float HighestSpeedKmh { get; private set; }

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
            ApplySelectedCarAndPaint();
        }

        public void ResetDriftScore() => DriftScore = 0f;

        public void ApplyImpact(float speedLoss = 0.4f)
        {
            if (body == null) return;
            body.velocity *= 1f - Mathf.Clamp01(speedLoss);
            body.AddForce(transform.right * Random.Range(-2.5f, 2.5f), ForceMode.Impulse);
        }

        private void ApplySelectedCarAndPaint()
        {
            var car = PlayerCareerSave.SelectedCar;
            var speed = new[] { 1f, 1.12f, 1.02f, 0.96f, 1.22f }[car];
            var acceleration = new[] { 1f, 1.08f, 1.02f, 1.14f, 1.2f }[car];
            var steering = new[] { 1f, 0.95f, 1.18f, 0.82f, 1.04f }[car];
            if (config != null)
            {
                config = Instantiate(config);
                config.maxSpeed *= speed;
                config.acceleration *= acceleration;
                config.steeringDegreesPerSecond *= steering;
                config.maxSpeed *= 1f + Mathf.Max(0, PlayerCareerSave.PartLevel(3) - 1) * 0.025f;
                config.acceleration *= 1f + Mathf.Max(0, PlayerCareerSave.PartLevel(1) - 1) * 0.035f
                    + Mathf.Max(0, PlayerCareerSave.PartLevel(2) - 1) * 0.025f
                    + Mathf.Max(0, PlayerCareerSave.PartLevel(3) - 1) * 0.02f;
                config.braking *= 1f + Mathf.Max(0, PlayerCareerSave.PartLevel(5) - 1) * 0.06f;
                config.lateralGrip *= 1f + Mathf.Max(0, PlayerCareerSave.PartLevel(4) - 1) * 0.05f
                    + Mathf.Max(0, PlayerCareerSave.PartLevel(6) - 1) * 0.025f;
                config.handbrakeGrip *= 1f + Mathf.Max(0, PlayerCareerSave.PartLevel(4) - 1) * 0.035f;
                config.steeringDegreesPerSecond *= 1f + Mathf.Max(0, PlayerCareerSave.PartLevel(6) - 1) * 0.03f;
                config.nitroCapacitySeconds += Mathf.Max(0, PlayerCareerSave.PartLevel(7) - 1) * 0.35f;
                config.nitroAcceleration *= 1f + Mathf.Max(0, PlayerCareerSave.PartLevel(7) - 1) * 0.08f;
            }

            if (ColorUtility.TryParseHtmlString(PlayerCareerSave.SelectedPaint, out var paint))
            {
                var bodyRenderer = transform.Find("Coupe Body")?.GetComponent<Renderer>();
                if (bodyRenderer != null) bodyRenderer.material.color = paint;
            }
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
            HighestSpeedKmh = Mathf.Max(HighestSpeedKmh, SpeedKmh);
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
            if (input.Handbrake && speedFactor > 0.25f && Mathf.Abs(input.Steering) > 0.15f)
                DriftScore += body.velocity.magnitude * 3.6f * Mathf.Abs(input.Steering) * Time.fixedDeltaTime;
            var turn = input.Steering * config.steeringDegreesPerSecond * speedFactor * Time.fixedDeltaTime;
            if (input.Handbrake) turn *= 1.55f;
            body.MoveRotation(body.rotation * Quaternion.Euler(0f, turn, 0f));
            var lateral = transform.right * Vector3.Dot(body.velocity, transform.right);
            var grip = input.Handbrake ? config.handbrakeGrip : config.lateralGrip;
            body.AddForce(-lateral * grip, ForceMode.Acceleration);
        }
    }
}
