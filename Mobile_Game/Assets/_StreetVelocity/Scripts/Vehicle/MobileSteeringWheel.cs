using StreetVelocity.Progression;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StreetVelocity.Vehicle
{
    public sealed class MobileSteeringWheel : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IPointerExitHandler
    {
        [SerializeField] private MobileVehicleInput input;
        private RectTransform rectTransform;

        public void Configure(MobileVehicleInput target) => input = target;

        private void Awake() => rectTransform = transform as RectTransform;
        public void OnPointerDown(PointerEventData eventData) => Steer(eventData);
        public void OnDrag(PointerEventData eventData) => Steer(eventData);
        public void OnPointerUp(PointerEventData eventData) => Release();
        public void OnPointerExit(PointerEventData eventData) => Release();

        private void Steer(PointerEventData eventData)
        {
            if (input == null || rectTransform == null) return;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out var local)) return;
            var normalized = Mathf.Clamp(local.x / Mathf.Max(1f, rectTransform.rect.width * 0.5f), -1f, 1f);
            input.SetSteering(normalized);
        }

        private void Release() => input?.SetSteering(0f);
    }
}
