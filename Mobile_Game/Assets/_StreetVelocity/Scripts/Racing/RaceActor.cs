using StreetVelocity.Vehicle;
using UnityEngine;

namespace StreetVelocity.Racing
{
    /// <summary>Moves a generated race car as a rival, ambient traffic vehicle, or pursuing police unit.</summary>
    [RequireComponent(typeof(Collider))]
    public sealed class RaceActor : MonoBehaviour
    {
        [SerializeField] private RaceManager raceManager;
        [SerializeField] private VehicleController player;
        [SerializeField] private float speed;
        [SerializeField] private float laneOffset;
        [SerializeField] private bool police;
        [SerializeField] private bool traffic;
        private float impactCooldown;
        private float raceStartZ;
        public bool IsPolice => police;

        public void Configure(RaceManager manager, VehicleController target, float metersPerSecond, float lane, bool isPolice, bool isTraffic)
        {
            raceManager = manager;
            player = target;
            raceStartZ = target != null ? target.transform.position.z : 0f;
            speed = metersPerSecond;
            laneOffset = lane;
            police = isPolice;
            traffic = isTraffic;
        }

        private void Update()
        {
            if (player == null) return;
            impactCooldown = Mathf.Max(0f, impactCooldown - Time.deltaTime);
            if (police)
            {
                var target = player.transform.position - player.transform.forward * 4.5f + player.transform.right * laneOffset;
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                var direction = (player.transform.position - transform.position).normalized;
                if (direction.sqrMagnitude > 0.01f) transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
                if (impactCooldown <= 0f && Vector3.Distance(transform.position, player.transform.position) < 5.3f)
                    ApplyImpact(0.26f);
                return;
            }

            var lateral = laneOffset + (traffic ? Mathf.Sin(Time.time * 0.25f + transform.GetInstanceID()) * 0.18f : Mathf.Sin(Time.time * 0.42f + transform.GetInstanceID()) * 0.62f);
            var targetX = Mathf.Clamp(lateral, -7.5f, 7.5f);
            transform.position += Vector3.forward * (speed * Time.deltaTime);
            transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, targetX, Time.deltaTime * 0.9f), transform.position.y, transform.position.z);
            if (!traffic) raceManager?.ReportRivalProgress(this, transform.position.z - raceStartZ);
            if (traffic && transform.position.z < player.transform.position.z - 24f)
            {
                transform.position = new Vector3(targetX, transform.position.y, player.transform.position.z + Random.Range(85f, 145f));
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && (traffic || police)) ApplyImpact(traffic ? 0.42f : 0.26f);
        }

        private void ApplyImpact(float reduction)
        {
            if (impactCooldown > 0f || player == null) return;
            impactCooldown = 1.4f;
            player.ApplyImpact(reduction);
        }
    }
}
