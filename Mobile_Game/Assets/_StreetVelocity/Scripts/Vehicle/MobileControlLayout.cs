using StreetVelocity.Progression;
using UnityEngine;

namespace StreetVelocity.Vehicle
{
    public sealed class MobileControlLayout : MonoBehaviour
    {
        [SerializeField] private GameObject leftButton;
        [SerializeField] private GameObject rightButton;
        [SerializeField] private GameObject steeringWheel;
        private int lastMode = -1;

        public void Configure(GameObject left, GameObject right, GameObject wheel)
        {
            leftButton = left;
            rightButton = right;
            steeringWheel = wheel;
        }

        private void Update()
        {
            var mode = PlayerCareerSave.ControlMode;
            if (mode == lastMode) return;
            lastMode = mode;
            if (leftButton != null) leftButton.SetActive(mode == 0);
            if (rightButton != null) rightButton.SetActive(mode == 0);
            if (steeringWheel != null) steeringWheel.SetActive(mode == 1);
        }
    }
}
