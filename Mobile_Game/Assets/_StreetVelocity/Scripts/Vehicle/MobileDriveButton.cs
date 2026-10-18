using UnityEngine;
using UnityEngine.EventSystems;

namespace StreetVelocity.Vehicle
{
    public sealed class MobileDriveButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        public enum DriveAction { SteerLeft, SteerRight, Throttle, Brake, Handbrake, Nitro }

        [SerializeField] private MobileVehicleInput input;
        [SerializeField] private DriveAction action;

        public void Configure(MobileVehicleInput target, DriveAction buttonAction)
        {
            input = target;
            action = buttonAction;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (input == null) return;
            switch (action)
            {
                case DriveAction.SteerLeft: input.SetSteering(-1f); break;
                case DriveAction.SteerRight: input.SetSteering(1f); break;
                case DriveAction.Throttle: input.SetThrottle(true); break;
                case DriveAction.Brake: input.SetBrake(true); break;
                case DriveAction.Handbrake: input.SetHandbrake(true); break;
                case DriveAction.Nitro: input.SetNitro(true); break;
            }
        }

        public void OnPointerUp(PointerEventData eventData) => Release();
        public void OnPointerExit(PointerEventData eventData) => Release();

        private void Release()
        {
            if (input == null) return;
            switch (action)
            {
                case DriveAction.SteerLeft:
                case DriveAction.SteerRight: input.SetSteering(0f); break;
                case DriveAction.Throttle: input.SetThrottle(false); break;
                case DriveAction.Brake: input.SetBrake(false); break;
                case DriveAction.Handbrake: input.SetHandbrake(false); break;
                case DriveAction.Nitro: input.SetNitro(false); break;
            }
        }

        private void OnDisable() => Release();
    }
}
