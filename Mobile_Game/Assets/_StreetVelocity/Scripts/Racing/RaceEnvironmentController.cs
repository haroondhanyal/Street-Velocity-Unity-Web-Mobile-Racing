using StreetVelocity.Progression;
using UnityEngine;

namespace StreetVelocity.Racing
{
    public sealed class RaceEnvironmentController : MonoBehaviour
    {
        private ParticleSystem rain;
        private Transform rainAnchor;

        private void Start()
        {
            var weather = PlayerCareerSave.Weather;
            RenderSettings.fog = weather != 0;
            RenderSettings.fogColor = weather == 3 ? new Color(0.45f, 0.55f, 0.64f) : new Color(0.035f, 0.075f, 0.12f);
            RenderSettings.fogDensity = weather == 3 ? 0.009f : weather == 2 ? 0.0035f : 0.0018f;
            RenderSettings.ambientLight = weather == 0 ? new Color(0.48f, 0.55f, 0.66f) : new Color(0.12f, 0.18f, 0.28f);
            if (weather == 2) CreateRain();
        }

        private void Update()
        {
            if (rain == null) return;
            if (rainAnchor == null) rainAnchor = Camera.main != null ? Camera.main.transform : transform;
            rain.transform.position = rainAnchor.position + rainAnchor.forward * 12f + Vector3.up * 12f;
        }

        private void CreateRain()
        {
            var rainObject = new GameObject("Rainfall");
            rainObject.transform.SetParent(transform, false);
            rain = rainObject.AddComponent<ParticleSystem>();
            var main = rain.main;
            main.loop = true;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.startLifetime = 1.2f;
            main.startSpeed = 34f;
            main.startSize = 0.045f;
            main.startColor = new Color(0.55f, 0.82f, 1f, 0.6f);
            main.maxParticles = 1800;
            var emission = rain.emission;
            emission.rateOverTime = 650f;
            var shape = rain.shape;
            shape.shapeType = ParticleSystemShapeType.Box;
            shape.scale = new Vector3(24f, 0.1f, 52f);
            var renderer = rain.GetComponent<ParticleSystemRenderer>();
            renderer.material = new Material(Shader.Find("Particles/Standard Unlit"));
            rain.Play();
        }
    }
}
