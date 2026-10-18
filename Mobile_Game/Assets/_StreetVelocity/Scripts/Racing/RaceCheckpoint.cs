using UnityEngine;

namespace StreetVelocity.Racing
{
    [RequireComponent(typeof(Collider))]
    public sealed class RaceCheckpoint : MonoBehaviour
    {
        [Min(0)] [SerializeField] private int order;
        [SerializeField] private RaceManager raceManager;
        public int Order => order;
        private void Reset() => GetComponent<Collider>().isTrigger = true;
        private void OnTriggerEnter(Collider other)
        {
            if (raceManager != null && other.CompareTag("Player")) raceManager.TryPassCheckpoint(this);
        }
    }
}
