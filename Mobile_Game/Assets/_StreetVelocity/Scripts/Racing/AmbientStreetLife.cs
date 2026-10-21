using System.Collections.Generic;
using UnityEngine;

namespace StreetVelocity.Racing
{
    /// <summary>Lightweight moving street traffic and pedestrians for the mobile driving scene.</summary>
    public sealed class AmbientStreetLife : MonoBehaviour
    {
        private static readonly Dictionary<string, Material> MaterialCache = new Dictionary<string, Material>();
        private readonly List<AmbientCar> cars = new List<AmbientCar>();
        private readonly List<AmbientPedestrian> pedestrians = new List<AmbientPedestrian>();
        private Transform player;
        private Material[] carPaints;
        private Material[] clothes;
        private Material tire;

        private void Start()
        {
            var playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject == null) return;
            player = playerObject.transform;
            carPaints = new[] { Paint("Taxi Yellow", new Color(0.95f, 0.62f, 0.08f)), Paint("Traffic Blue", new Color(0.08f, 0.3f, 0.65f)),
                Paint("Traffic Silver", new Color(0.58f, 0.65f, 0.7f)), Paint("Traffic Green", new Color(0.08f, 0.42f, 0.26f)), Paint("Traffic Red", new Color(0.67f, 0.08f, 0.12f)) };
            clothes = new[] { Paint("Jacket Teal", new Color(0.05f, 0.42f, 0.47f)), Paint("Jacket Orange", new Color(0.82f, 0.29f, 0.08f)),
                Paint("Jacket Violet", new Color(0.36f, 0.17f, 0.55f)), Paint("Jacket Cream", new Color(0.7f, 0.61f, 0.39f)), Paint("Jacket Blue", new Color(0.08f, 0.2f, 0.48f)) };
            tire = Paint("Street Tire", new Color(0.025f, 0.03f, 0.04f));

            for (var i = 0; i < 8; i++)
            {
                var opposing = i % 2 == 1;
                var car = BuildCar(i, opposing);
                var spawnZ = player.position.z + 24f + (i / 2) * 24f + Random.Range(-6f, 6f);
                var position = car.transform.position;
                position.z = spawnZ;
                car.transform.position = position;
                cars.Add(new AmbientCar(car, opposing ? -1f : 1f, opposing ? -3.1f : 3.1f,
                    Random.Range(8f, 15f)));
            }
            for (var i = 0; i < 12; i++)
            {
                var side = i % 2 == 0 ? -1f : 1f;
                pedestrians.Add(BuildPedestrian(i, side));
            }
        }

        private void Update()
        {
            if (player == null) return;
            var playerZ = player.position.z;
            foreach (var car in cars)
            {
                var position = car.root.transform.position;
                position.z += car.direction * car.speed * Time.deltaTime;
                if (position.z < playerZ - 100f || position.z > playerZ + 115f)
                    position.z = playerZ + Random.Range(55f, 105f);
                position.x = car.lane;
                car.root.transform.position = position;
            }
            foreach (var person in pedestrians)
            {
                var position = person.root.transform.position;
                position.z += person.speed * Time.deltaTime;
                if (position.z < playerZ - 100f || position.z > playerZ + 115f)
                    position.z = playerZ + Random.Range(-85f, 105f);
                person.root.transform.position = position;
                var stride = Mathf.Sin(Time.time * 7f + person.phase) * 22f;
                person.leftLeg.localRotation = Quaternion.Euler(stride, 0f, 0f);
                person.rightLeg.localRotation = Quaternion.Euler(-stride, 0f, 0f);
                person.leftArm.localRotation = Quaternion.Euler(-stride * 0.65f, 0f, 0f);
                person.rightArm.localRotation = Quaternion.Euler(stride * 0.65f, 0f, 0f);
            }
        }

        private GameObject BuildCar(int index, bool opposing)
        {
            var root = new GameObject("Ambient City Traffic " + (index + 1));
            root.transform.SetParent(transform, false);
            root.transform.rotation = Quaternion.Euler(0f, opposing ? 180f : 0f, 0f);
            root.transform.position = new Vector3(opposing ? -3.1f : 3.1f, 0.15f, player.position.z + Random.Range(-42f, 95f));
            var length = index % 3 == 0 ? 4.25f : 3.55f;
            var width = index % 3 == 0 ? 1.78f : 1.64f;
            var paint = carPaints[index % carPaints.Length];
            Part("Body", PrimitiveType.Cube, root.transform, new Vector3(0f, 0.62f, 0f), new Vector3(width, 0.62f, length), paint);
            Part("Cabin", PrimitiveType.Cube, root.transform, new Vector3(0f, 1.04f, -0.12f), new Vector3(width * 0.76f, 0.48f, length * 0.48f),
                Paint("Traffic Glass", new Color(0.035f, 0.085f, 0.12f)));
            for (var side = -1; side <= 1; side += 2)
            for (var axle = -1; axle <= 1; axle += 2)
                Part("Wheel", PrimitiveType.Cylinder, root.transform, new Vector3(side * width * 0.52f, 0.39f, axle * length * 0.31f),
                    new Vector3(0.37f, 0.14f, 0.37f), tire).transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            var lamp = Paint("Traffic Headlights", new Color(0.9f, 0.82f, 0.59f), new Color(0.75f, 0.63f, 0.38f));
            Part("Headlights", PrimitiveType.Cube, root.transform, new Vector3(0f, 0.66f, length * 0.5f), new Vector3(width * 0.72f, 0.13f, 0.06f), lamp);
            Part("Tail lights", PrimitiveType.Cube, root.transform, new Vector3(0f, 0.66f, -length * 0.5f), new Vector3(width * 0.72f, 0.13f, 0.06f),
                Paint("Traffic Tail Lights", new Color(0.65f, 0.035f, 0.025f), new Color(0.7f, 0.015f, 0.01f)));
            var trigger = root.AddComponent<BoxCollider>();
            trigger.isTrigger = true;
            trigger.size = new Vector3(width, 1.2f, length);
            return root;
        }

        private AmbientPedestrian BuildPedestrian(int index, float side)
        {
            var root = new GameObject("City Pedestrian " + (index + 1));
            root.transform.SetParent(transform, false);
            root.transform.position = new Vector3(side * Random.Range(11.6f, 14.8f), 0f, player.position.z + Random.Range(-90f, 105f));
            var skin = Paint("Pedestrian Skin", new Color(0.64f, 0.39f, 0.25f));
            var outfit = clothes[index % clothes.Length];
            Part("Torso", PrimitiveType.Capsule, root.transform, new Vector3(0f, 1.08f, 0f), new Vector3(0.44f, 0.43f, 0.3f), outfit);
            Part("Head", PrimitiveType.Sphere, root.transform, new Vector3(0f, 1.78f, 0.01f), new Vector3(0.3f, 0.34f, 0.3f), skin);
            var leftArm = Part("Left Arm", PrimitiveType.Capsule, root.transform, new Vector3(-0.3f, 1.08f, 0f), new Vector3(0.14f, 0.34f, 0.14f), outfit).transform;
            var rightArm = Part("Right Arm", PrimitiveType.Capsule, root.transform, new Vector3(0.3f, 1.08f, 0f), new Vector3(0.14f, 0.34f, 0.14f), outfit).transform;
            var leftLeg = Part("Left Leg", PrimitiveType.Capsule, root.transform, new Vector3(-0.13f, 0.46f, 0f), new Vector3(0.16f, 0.34f, 0.16f), Paint("Pedestrian Trousers", new Color(0.11f, 0.15f, 0.2f))).transform;
            var rightLeg = Part("Right Leg", PrimitiveType.Capsule, root.transform, new Vector3(0.13f, 0.46f, 0f), new Vector3(0.16f, 0.34f, 0.16f), Paint("Pedestrian Shoes", new Color(0.07f, 0.065f, 0.06f))).transform;
            var speed = Random.Range(0.55f, 1.05f) * (Random.value < 0.5f ? -1f : 1f);
            root.transform.rotation = Quaternion.Euler(0f, speed < 0f ? 180f : 0f, 0f);
            return new AmbientPedestrian(root, speed, leftLeg, rightLeg, leftArm, rightArm);
        }

        private static GameObject Part(string name, PrimitiveType type, Transform parent, Vector3 position, Vector3 scale, Material material)
        {
            var part = GameObject.CreatePrimitive(type);
            part.name = name;
            part.transform.SetParent(parent, false);
            part.transform.localPosition = position;
            part.transform.localScale = scale;
            var collider = part.GetComponent<Collider>();
            if (collider != null) Object.Destroy(collider);
            part.GetComponent<Renderer>().sharedMaterial = material;
            return part;
        }

        private static Material Paint(string name, Color color, Color? emission = null)
        {
            if (MaterialCache.TryGetValue(name, out var cached) && cached != null) return cached;
            var shader = Shader.Find("Standard");
            if (shader == null) shader = Shader.Find("Universal Render Pipeline/Lit");
            var material = new Material(shader) { name = name, color = color };
            if (emission.HasValue)
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", emission.Value);
            }
            MaterialCache[name] = material;
            return material;
        }

        private sealed class AmbientCar
        {
            public readonly GameObject root; public readonly float direction, lane, speed;
            public AmbientCar(GameObject value, float direction, float lane, float speed)
            { root = value; this.direction = direction; this.lane = lane; this.speed = speed; }
        }

        private sealed class AmbientPedestrian
        {
            public readonly GameObject root; public readonly float speed, phase;
            public readonly Transform leftLeg, rightLeg, leftArm, rightArm;
            public AmbientPedestrian(GameObject value, float speed, Transform leftLeg, Transform rightLeg, Transform leftArm, Transform rightArm)
            {
                root = value; this.speed = speed; phase = Random.Range(0f, 6f);
                this.leftLeg = leftLeg; this.rightLeg = rightLeg; this.leftArm = leftArm; this.rightArm = rightArm;
            }
        }
    }
}
