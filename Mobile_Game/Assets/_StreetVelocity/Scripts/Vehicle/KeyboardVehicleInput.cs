using UnityEngine;

namespace StreetVelocity.Vehicle
{
    /// <summary>Editor/desktop fallback. Mobile input should implement the same interface.</summary>
    public sealed class KeyboardVehicleInput : MonoBehaviour, IVehicleInput
    {
        public float Throttle => Mathf.Clamp01(Input.GetAxisRaw("Vertical"));
        public float Steering => Input.GetAxisRaw("Horizontal");
        public bool Brake => Input.GetKey(KeyCode.Space) || Input.GetAxisRaw("Vertical") < 0f;
        public bool Handbrake => Input.GetKey(KeyCode.LeftShift);
        public bool Nitro => Input.GetKey(KeyCode.N);
    }
}
