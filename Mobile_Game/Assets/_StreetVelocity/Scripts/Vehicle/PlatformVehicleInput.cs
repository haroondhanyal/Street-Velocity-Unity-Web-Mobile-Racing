using UnityEngine;

namespace StreetVelocity.Vehicle
{
    /// <summary>Uses touch controls on phones and keyboard input in the Editor or desktop build.</summary>
    public sealed class PlatformVehicleInput : MonoBehaviour, IVehicleInput
    {
        [SerializeField] private MobileVehicleInput mobileInput;
        [SerializeField] private KeyboardVehicleInput keyboardInput;

        private bool UseKeyboard => !Application.isMobilePlatform;
        public float Throttle => Mathf.Max(mobileInput == null ? 0f : mobileInput.Throttle,
            UseKeyboard && keyboardInput != null ? keyboardInput.Throttle : 0f);
        public float Steering => Mathf.Clamp((mobileInput == null ? 0f : mobileInput.Steering) +
            (UseKeyboard && keyboardInput != null ? keyboardInput.Steering : 0f), -1f, 1f);
        public bool Brake => (mobileInput != null && mobileInput.Brake) || (UseKeyboard && keyboardInput != null && keyboardInput.Brake);
        public bool Handbrake => (mobileInput != null && mobileInput.Handbrake) || (UseKeyboard && keyboardInput != null && keyboardInput.Handbrake);
        public bool Nitro => (mobileInput != null && mobileInput.Nitro) || (UseKeyboard && keyboardInput != null && keyboardInput.Nitro);

        private void Awake()
        {
            if (mobileInput == null) mobileInput = GetComponent<MobileVehicleInput>();
            if (keyboardInput == null) keyboardInput = GetComponent<KeyboardVehicleInput>();
        }
    }
}
