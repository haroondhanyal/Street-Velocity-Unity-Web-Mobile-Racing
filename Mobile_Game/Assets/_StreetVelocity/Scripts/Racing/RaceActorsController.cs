using System.Collections.Generic;
using StreetVelocity.Vehicle;
using UnityEngine;

namespace StreetVelocity.Racing
{
    /// <summary>Spawns simple moving rivals, traffic, or police pursuers for the generated prototype route.</summary>
    public sealed class RaceActorsController : MonoBehaviour
    {
        [SerializeField] private RaceManager raceManager;
        [SerializeField] private VehicleController player;
        private readonly List<RaceActor> actors = new List<RaceActor>();
        private bool spawned;

        public void Configure(RaceManager manager, VehicleController vehicle)
        {
            raceManager = manager;
            player = vehicle;
        }

        private void Start()
        {
            if (raceManager == null) return;
            raceManager.RaceStarted += SpawnActors;
            raceManager.Finished += StopActors;
        }

        private void OnDisable()
        {
            if (raceManager == null) return;
            raceManager.RaceStarted -= SpawnActors;
            raceManager.Finished -= StopActors;
        }

        private void StopActors(float _) 
        {
            foreach (var actor in actors)
                if (actor != null) actor.enabled = false;
        }

        private void Update()
        {
            if (raceManager == null || player == null || !raceManager.IsPoliceEscape || raceManager.Phase != RacePhase.Racing) return;
            var closest = float.MaxValue;
            foreach (var actor in actors)
                if (actor != null && actor.IsPolice)
                    closest = Mathf.Min(closest, Vector3.Distance(actor.transform.position, player.transform.position));
            raceManager.UpdatePolicePressure(closest, Time.deltaTime);
        }

        private void SpawnActors()
        {
            if (spawned || raceManager == null || player == null) return;
            spawned = true;
            var eventIndex = raceManager.EventIndex;
            if (eventIndex == 0 || eventIndex == 1)
            {
                var rivalColors = new[] { new Color(0.04f, 0.56f, 0.82f), new Color(0.8f, 0.05f, 0.18f), new Color(0.48f, 0.25f, 0.82f), new Color(0.9f, 0.48f, 0.04f) };
                for (var i = 0; i < 4; i++)
                    CreateActor($"AI Rival {i + 1}", false, false, rivalColors[i], 48f + i * 3f,
                        player.transform.position + Vector3.forward * (22f + i * 14f), (i % 2 == 0 ? -1f : 1f) * (2.4f + (i % 3) * 2.4f));
            }

            if (eventIndex == 4)
            {
                for (var i = 0; i < 2; i++)
                    CreateActor($"Police Pursuit Unit {i + 1}", true, false, new Color(0.035f, 0.07f, 0.13f), 42f + i * 5f,
                        player.transform.position - Vector3.forward * (30f + i * 16f), i == 0 ? -2.7f : 2.7f);
            }
            else
            {
                var trafficColors = new[] { new Color(0.5f, 0.56f, 0.62f), new Color(0.82f, 0.42f, 0.06f), new Color(0.12f, 0.42f, 0.56f), new Color(0.72f, 0.72f, 0.69f) };
                for (var i = 0; i < 4; i++)
                    CreateActor($"City Traffic {i + 1}", false, true, trafficColors[i], 15f + i * 2.2f,
                        player.transform.position + Vector3.forward * (45f + i * 38f), (i % 2 == 0 ? -1f : 1f) * (2.3f + (i % 3) * 2.7f));
            }
        }

        private void CreateActor(string actorName, bool police, bool traffic, Color color, float speed, Vector3 position, float lane)
        {
            var root = new GameObject(actorName, typeof(BoxCollider), typeof(RaceActor));
            root.transform.SetParent(transform, true);
            root.transform.position = position;
            var trigger = root.GetComponent<BoxCollider>();
            trigger.isTrigger = true;
            trigger.size = new Vector3(1.9f, 1.15f, 3.7f);

            var body = Primitive(traffic ? "Traffic Body" : "Racer Body", PrimitiveType.Cube, root.transform,
                new Vector3(0f, 0.55f, 0f), new Vector3(1.82f, 0.62f, 3.65f), color);
            body.transform.localRotation = Quaternion.identity;
            Primitive("Cabin", PrimitiveType.Cube, root.transform, new Vector3(0f, 1.02f, -0.12f),
                new Vector3(1.34f, 0.42f, 1.62f), police ? new Color(0.025f, 0.045f, 0.075f) : new Color(0.045f, 0.12f, 0.18f));
            for (var side = -1; side <= 1; side += 2)
            {
                var lightColor = police ? (side < 0 ? Color.red : Color.cyan) : traffic ? new Color(1f, 0.7f, 0.26f) : Color.red;
                Primitive("Rear Lamp", PrimitiveType.Cube, root.transform, new Vector3(side * 0.58f, 0.62f, -1.82f),
                    new Vector3(0.42f, 0.13f, 0.06f), lightColor);
                Primitive("Front Lamp", PrimitiveType.Cube, root.transform, new Vector3(side * 0.58f, 0.62f, 1.82f),
                    new Vector3(0.3f, 0.1f, 0.06f), police ? (side < 0 ? Color.red : Color.cyan) : Color.white);
            }
            var actor = root.GetComponent<RaceActor>();
            actor.Configure(raceManager, player, speed, lane, police, traffic);
            actors.Add(actor);
            if (!traffic && !police) raceManager.RegisterRival(actor);
        }

        private static GameObject Primitive(string name, PrimitiveType type, Transform parent, Vector3 localPosition, Vector3 scale, Color color)
        {
            var part = GameObject.CreatePrimitive(type);
            part.name = name;
            part.transform.SetParent(parent, false);
            part.transform.localPosition = localPosition;
            part.transform.localScale = scale;
            var collider = part.GetComponent<Collider>();
            if (collider != null) collider.enabled = false;
            var renderer = part.GetComponent<Renderer>();
            renderer.material.color = color;
            if (name.Contains("Lamp")) renderer.material.EnableKeyword("_EMISSION");
            return part;
        }
    }
}
