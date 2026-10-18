using UnityEngine;

namespace StreetVelocity.Vehicle
{
    public sealed class VehicleCameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0f, 5.2f, -10.5f);
        [SerializeField, Min(0f)] private float followSmoothness = 7f;
        [SerializeField, Min(0f)] private float lookHeight = 1.1f;

        public void SetTarget(Transform value) => target = value;

        private void LateUpdate()
        {
            if (target == null) return;
            var desired = target.TransformPoint(offset);
            transform.position = Vector3.Lerp(transform.position, desired, 1f - Mathf.Exp(-followSmoothness * Time.deltaTime));
            transform.LookAt(target.position + Vector3.up * lookHeight + target.forward * 6f);
        }
    }
}
