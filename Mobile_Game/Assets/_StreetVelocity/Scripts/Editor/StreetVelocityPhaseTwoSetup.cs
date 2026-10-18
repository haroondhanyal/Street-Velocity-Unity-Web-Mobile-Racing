#if UNITY_EDITOR
using System.Collections.Generic;
using StreetVelocity.Racing;
using StreetVelocity.UI;
using StreetVelocity.Vehicle;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace StreetVelocity.EditorTools
{
    /// <summary>Builds a self-contained arcade driving scene for keyboard and touch input.</summary>
    public static class StreetVelocityPhaseTwoSetup
    {
        private const string ScenePath = "Assets/_StreetVelocity/Scenes/09_DriveTest.unity";
        private const string ConfigPath = "Assets/_StreetVelocity/ScriptableObjects/Cars/FalconR_Drive.asset";
        private const string RacePath = "Assets/_StreetVelocity/ScriptableObjects/Races/DowntownSprint.asset";
        private const string MaterialFolder = "Assets/_StreetVelocity/Art/Materials";
        private static readonly Dictionary<string, Material> MaterialCache = new Dictionary<string, Material>();

        [MenuItem("Street Velocity/Phase 2/Build Driving Test Scene")]
        public static void BuildScene()
        {
            EnsureFolder("Assets/_StreetVelocity/ScriptableObjects/Cars");
            EnsureFolder("Assets/_StreetVelocity/Scenes");
            EnsureFolder(MaterialFolder);
            AssetDatabase.Refresh();
            var config = GetVehicleConfig();
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(ScenePath) != null) AssetDatabase.DeleteAsset(ScenePath);
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            CreateLighting();
            CreateStreet();
            var mobileInput = CreateVehicle(config, out var vehicle, out var cameraFollow);
            var raceManager = CreateRaceSystem(vehicle);
            var raceActors = new GameObject("Race Actors").AddComponent<RaceActorsController>();
            raceActors.transform.SetParent(GameObject.Find("Neon City Driving Test").transform, false);
            raceActors.Configure(raceManager, vehicle);
            CreateHud(vehicle, mobileInput, raceManager);
            EditorSceneManager.SaveScene(scene, ScenePath);
            AddBuildScene(ScenePath);
            EditorSceneManager.OpenScene(ScenePath);
            AssetDatabase.SaveAssets();
            Debug.Log("Phase 2 driving scene created. Use W/A/S/D or arrow keys, Space to brake, Left Shift for handbrake, and N for nitro. Touch controls are included.");
        }

        private static VehicleConfig GetVehicleConfig()
        {
            var config = AssetDatabase.LoadAssetAtPath<VehicleConfig>(ConfigPath);
            if (config != null) return config;
            config = ScriptableObject.CreateInstance<VehicleConfig>();
            config.acceleration = 19f;
            config.reverseAcceleration = 9f;
            config.braking = 30f;
            config.maxSpeed = 88f;
            config.steeringDegreesPerSecond = 82f;
            config.lateralGrip = 2.1f;
            config.nitroAcceleration = 32f;
            config.nitroCapacitySeconds = 3.4f;
            config.handbrakeGrip = 0.12f;
            AssetDatabase.CreateAsset(config, ConfigPath);
            AssetDatabase.SaveAssets();
            return config;
        }

        private static void CreateStreet()
        {
            var environment = new GameObject("Neon City Driving Test");
            environment.AddComponent<RaceEnvironmentController>();
            var ground = Material("Wet Asphalt", new Color(0.025f, 0.045f, 0.075f), 0.3f);
            var asphalt = Material("Road Asphalt", new Color(0.055f, 0.07f, 0.095f), 0.45f);
            var cyan = Material("Neon Cyan", new Color(0.02f, 0.55f, 0.8f), 0.25f, new Color(0.05f, 0.9f, 1f));
            var pink = Material("Neon Magenta", new Color(0.7f, 0.03f, 0.22f), 0.2f, new Color(1f, 0.035f, 0.24f));
            var lane = Material("Road Markings", new Color(0.55f, 0.68f, 0.78f), 0.05f, new Color(0.15f, 0.28f, 0.34f));
            Cube("City Floor", new Vector3(0f, -0.65f, 1100f), new Vector3(280f, 0.5f, 2250f), ground, environment.transform);
            Cube("Main Street", new Vector3(0f, -0.18f, 1100f), new Vector3(19f, 0.25f, 2220f), asphalt, environment.transform);
            Cube("Left Neon Curb", new Vector3(-9.65f, 0.02f, 1100f), new Vector3(0.18f, 0.22f, 2220f), cyan, environment.transform);
            Cube("Right Neon Curb", new Vector3(9.65f, 0.02f, 1100f), new Vector3(0.18f, 0.22f, 2220f), pink, environment.transform);
            Cube("Left Safety Barrier", new Vector3(-10.7f, 0.55f, 1100f), new Vector3(0.4f, 1.2f, 2220f), ground, environment.transform);
            Cube("Right Safety Barrier", new Vector3(10.7f, 0.55f, 1100f), new Vector3(0.4f, 1.2f, 2220f), ground, environment.transform);
            for (var z = 8f; z < 2200f; z += 17f)
            {
                Cube("Lane Dash", new Vector3(0f, -0.045f, z), new Vector3(0.18f, 0.03f, 7.5f), lane, environment.transform);
                BuildSideBlock(z, -1, environment.transform, cyan, pink);
                BuildSideBlock(z + 8f, 1, environment.transform, pink, cyan);
                if (((int)z) % 68 == 8) BuildStreetLight(z, environment.transform, cyan, pink);
            }
            RenderSettings.fog = true;
            RenderSettings.fogColor = new Color(0.035f, 0.075f, 0.12f);
            RenderSettings.fogDensity = 0.003f;
        }

        private static void BuildSideBlock(float z, int side, Transform parent, Material first, Material second)
        {
            var height = 12f + Mathf.Abs(Mathf.Sin(z * 0.16f)) * 28f;
            var width = 5.5f + Mathf.Abs(Mathf.Cos(z * 0.09f)) * 7f;
            var x = side * (17f + width * 0.56f);
            var building = Cube("Neon City Tower", new Vector3(x, height * 0.5f - 0.15f, z),
                new Vector3(width, height, 13f + (z % 3f)), Material("Tower Glass", new Color(0.035f, 0.075f, 0.13f), 0.8f), parent);
            building.transform.localScale = new Vector3(width, height, 13f + Mathf.Abs(Mathf.Sin(z)) * 8f);
            if (((int)z) % 3 == 0)
            {
                var strip = Cube("Tower Neon Edge", new Vector3(x - side * width * 0.38f, height * 0.52f, z),
                    new Vector3(0.12f, height * 0.8f, 0.2f), ((int)z) % 2 == 0 ? first : second, parent);
                strip.transform.SetParent(building.transform, true);
            }
        }

        private static void BuildStreetLight(float z, Transform parent, Material cyan, Material pink)
        {
            for (var side = -1; side <= 1; side += 2)
            {
                var pole = Cube("Street Light Pole", new Vector3(side * 12f, 4f, z), new Vector3(0.18f, 8f, 0.18f),
                    Material("Street Lamp Metal", new Color(0.12f, 0.17f, 0.22f), 0.8f), parent);
                Cube("Street Light Arm", new Vector3(side * 10.8f, 7.8f, z), new Vector3(2.5f, 0.14f, 0.16f), cyan, pole.transform);
                Cube("Street Light Bar", new Vector3(side * 10.1f, 7.7f, z), new Vector3(1.1f, 0.1f, 0.45f), pink, pole.transform);
            }
        }

        private static MobileVehicleInput CreateVehicle(VehicleConfig config, out VehicleController controller, out VehicleCameraFollow cameraFollow)
        {
            var root = new GameObject("Player Street Racer");
            root.tag = "Player";
            root.transform.position = new Vector3(0f, 0.65f, 4f);
            var body = root.AddComponent<Rigidbody>();
            body.mass = 1180f;
            body.drag = 0.08f;
            body.angularDrag = 3.5f;
            body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            var bodyCollider = root.AddComponent<BoxCollider>();
            bodyCollider.center = new Vector3(0f, 0.52f, 0f);
            bodyCollider.size = new Vector3(1.85f, 0.5f, 3.9f);
            var shell = Cube("Coupe Body", Vector3.zero, new Vector3(1.85f, 0.62f, 3.9f), Material("Falcon R Red", new Color(0.72f, 0.018f, 0.09f), 0.7f), root.transform);
            shell.tag = "Player";
            shell.GetComponent<Collider>().enabled = false;
            shell.transform.localPosition = new Vector3(0f, 0.56f, 0f);
            var roof = Cube("Black Roof", Vector3.zero, new Vector3(1.36f, 0.44f, 1.68f), Material("Roof Glass", new Color(0.015f, 0.035f, 0.055f), 0.9f), root.transform);
            roof.transform.localPosition = new Vector3(0f, 1.02f, -0.12f);
            var splitter = Cube("Front Splitter", Vector3.zero, new Vector3(2.08f, 0.12f, 0.35f), Material("Carbon", new Color(0.018f, 0.026f, 0.036f), 0.45f), root.transform);
            splitter.transform.localPosition = new Vector3(0f, 0.37f, 2.02f);
            var wing = Cube("Rear Wing", Vector3.zero, new Vector3(2.15f, 0.1f, 0.35f), Material("Wing Carbon", new Color(0.02f, 0.03f, 0.04f), 0.5f), root.transform);
            wing.transform.localPosition = new Vector3(0f, 1.05f, -1.78f);
            for (var side = -1; side <= 1; side += 2)
            for (var axle = -1; axle <= 1; axle += 2)
            {
                var wheel = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                wheel.name = "Performance Tire";
                wheel.transform.SetParent(root.transform, false);
                wheel.transform.localPosition = new Vector3(side * 0.94f, 0.52f, axle * 1.28f);
                wheel.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
                wheel.transform.localScale = new Vector3(0.5f, 0.19f, 0.5f);
                wheel.GetComponent<Collider>().enabled = false;
                wheel.GetComponent<Renderer>().sharedMaterial = Material("Rubber Tire", new Color(0.012f, 0.016f, 0.02f), 0.1f);
            }
            var cyan = Material("Headlight Cyan", Color.white, 0.1f, new Color(0.35f, 0.82f, 1f));
            var leftLamp = Cube("Left Headlight", Vector3.zero, new Vector3(0.48f, 0.12f, 0.08f), cyan, root.transform);
            leftLamp.transform.localPosition = new Vector3(-0.57f, 0.66f, 1.98f);
            var rightLamp = Cube("Right Headlight", Vector3.zero, new Vector3(0.48f, 0.12f, 0.08f), cyan, root.transform);
            rightLamp.transform.localPosition = new Vector3(0.57f, 0.66f, 1.98f);

            var mobileInput = root.AddComponent<MobileVehicleInput>();
            root.AddComponent<KeyboardVehicleInput>();
            var platformInput = root.AddComponent<PlatformVehicleInput>();
            var inputSerialized = new SerializedObject(platformInput);
            inputSerialized.FindProperty("mobileInput").objectReferenceValue = mobileInput;
            inputSerialized.FindProperty("keyboardInput").objectReferenceValue = root.GetComponent<KeyboardVehicleInput>();
            inputSerialized.ApplyModifiedPropertiesWithoutUndo();
            controller = root.AddComponent<VehicleController>();
            var vehicleSerialized = new SerializedObject(controller);
            vehicleSerialized.FindProperty("config").objectReferenceValue = config;
            vehicleSerialized.FindProperty("inputSource").objectReferenceValue = platformInput;
            vehicleSerialized.ApplyModifiedPropertiesWithoutUndo();

            var cameraObject = new GameObject("Chase Camera", typeof(Camera), typeof(AudioListener), typeof(VehicleCameraFollow));
            cameraObject.tag = "MainCamera";
            cameraObject.transform.position = new Vector3(0f, 6f, -12f);
            cameraObject.transform.LookAt(root.transform.position + Vector3.forward * 6f);
            cameraFollow = cameraObject.GetComponent<VehicleCameraFollow>();
            var cameraSerialized = new SerializedObject(cameraFollow);
            cameraSerialized.FindProperty("target").objectReferenceValue = root.transform;
            cameraSerialized.ApplyModifiedPropertiesWithoutUndo();
            return mobileInput;
        }

        private static RaceManager CreateRaceSystem(VehicleController vehicle)
        {
            EnsureFolder("Assets/_StreetVelocity/ScriptableObjects/Races");
            var definition = AssetDatabase.LoadAssetAtPath<RaceDefinition>(RacePath);
            if (definition == null)
            {
                definition = ScriptableObject.CreateInstance<RaceDefinition>();
                AssetDatabase.CreateAsset(definition, RacePath);
            }
            definition.laps = 1;
            definition.checkpointCount = 4;
            definition.countdownSeconds = 3f;
            definition.creditsReward = 8000;
            definition.xpReward = 1200;
            EditorUtility.SetDirty(definition);

            var managerObject = new GameObject("Race Manager");
            var manager = managerObject.AddComponent<RaceManager>();
            var serialized = new SerializedObject(manager);
            serialized.FindProperty("definition").objectReferenceValue = definition;
            serialized.FindProperty("vehicle").objectReferenceValue = vehicle;
            serialized.ApplyModifiedPropertiesWithoutUndo();

            var cyan = Material("Checkpoint Cyan", new Color(0.02f, 0.55f, 0.8f), 0.25f, new Color(0.05f, 0.9f, 1f));
            var pink = Material("Checkpoint Magenta", new Color(0.7f, 0.03f, 0.22f), 0.2f, new Color(1f, 0.035f, 0.24f));
            for (var lap = 0; lap < 3; lap++)
            for (var order = 0; order < 4; order++)
                CreateCheckpoint(order, 145f + lap * 700f + order * 160f, manager, cyan, pink);
            return manager;
        }

        private static void CreateCheckpoint(int order, float z, RaceManager manager, Material cyan, Material pink)
        {
            var gate = new GameObject($"Checkpoint {order + 1:00}", typeof(BoxCollider), typeof(RaceCheckpoint));
            gate.transform.position = new Vector3(0f, 2.4f, z);
            var trigger = gate.GetComponent<BoxCollider>();
            trigger.isTrigger = true;
            trigger.size = new Vector3(18.8f, 5f, 1.5f);
            var checkpoint = gate.GetComponent<RaceCheckpoint>();
            var serialized = new SerializedObject(checkpoint);
            serialized.FindProperty("order").intValue = order;
            serialized.FindProperty("raceManager").objectReferenceValue = manager;
            serialized.ApplyModifiedPropertiesWithoutUndo();

            foreach (var side in new[] { -1f, 1f })
            {
                var post = Cube("Checkpoint Neon Post", new Vector3(side * 9.1f, 2.45f, z), new Vector3(0.28f, 4.8f, 0.28f), side < 0 ? cyan : pink, gate.transform);
                post.GetComponent<Collider>().enabled = false;
            }
            var beam = Cube("Checkpoint Neon Beam", new Vector3(0f, 4.8f, z), new Vector3(18.5f, 0.22f, 0.26f), cyan, gate.transform);
            beam.GetComponent<Collider>().enabled = false;
        }

        private static void CreateHud(VehicleController vehicle, MobileVehicleInput input, RaceManager raceManager)
        {
            var canvasObject = new GameObject("Driving HUD", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            var canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 5;
            var scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.45f;
            EnsureEventSystem();

            var hud = canvasObject.AddComponent<RaceHudController>();
            var speed = MakeText(canvas.transform, "Speed", "000", 70, new Vector2(0.88f, 0.82f), new Vector2(260f, 100f), TextAnchor.MiddleCenter);
            var gear = MakeText(canvas.transform, "Gear", "1", 28, new Vector2(0.88f, 0.72f), new Vector2(200f, 45f), TextAnchor.MiddleCenter);
            var timer = MakeText(canvas.transform, "Race Timer", "00:00.000", 27, new Vector2(0.15f, 0.93f), new Vector2(270f, 55f), TextAnchor.MiddleLeft);
            var position = MakeText(canvas.transform, "Race Position", "POS  1 / 5", 24, new Vector2(0.15f, 0.975f), new Vector2(280f, 42f), TextAnchor.MiddleLeft);
            var lap = MakeText(canvas.transform, "Lap Progress", "LAP 1 / 1", 23, new Vector2(0.15f, 0.875f), new Vector2(270f, 42f), TextAnchor.MiddleLeft);
            var checkpoint = MakeText(canvas.transform, "Checkpoint Progress", "CHECKPOINT 1 / 4", 20, new Vector2(0.15f, 0.83f), new Vector2(310f, 40f), TextAnchor.MiddleLeft);
            MakeText(canvas.transform, "District", "NEON DISTRICT  /  DOWNTOWN", 22, new Vector2(0.5f, 0.94f), new Vector2(560f, 52f), TextAnchor.MiddleCenter);
            MakeText(canvas.transform, "Controls Hint", "WASD / ARROWS · SPACE BRAKE · SHIFT DRIFT · N NITRO", 18, new Vector2(0.5f, 0.04f), new Vector2(650f, 42f), TextAnchor.MiddleCenter);
            CreateMenuButton(canvas.transform);
            var nitroBack = MakeImage(canvas.transform, "Nitro Meter Back", new Color(0.02f, 0.08f, 0.14f, 0.82f), new Vector2(0.88f, 0.62f), new Vector2(260f, 18f));
            var nitroFill = MakeImage(nitroBack.transform, "Nitro Meter Fill", new Color(0.03f, 0.78f, 1f, 1f), new Vector2(0f, 0.5f), new Vector2(1f, 1f));
            nitroFill.rectTransform.anchorMin = Vector2.zero;
            nitroFill.rectTransform.anchorMax = Vector2.one;
            nitroFill.rectTransform.offsetMin = Vector2.zero;
            nitroFill.rectTransform.offsetMax = Vector2.zero;
            nitroFill.type = Image.Type.Filled;
            nitroFill.fillMethod = Image.FillMethod.Horizontal;
            nitroFill.fillOrigin = (int)Image.OriginHorizontal.Left;
            var hudSerialized = new SerializedObject(hud);
            hudSerialized.FindProperty("vehicle").objectReferenceValue = vehicle;
            hudSerialized.FindProperty("speedText").objectReferenceValue = speed;
            hudSerialized.FindProperty("gearText").objectReferenceValue = gear;
            hudSerialized.FindProperty("timerText").objectReferenceValue = timer;
            hudSerialized.FindProperty("positionText").objectReferenceValue = position;
            hudSerialized.FindProperty("lapText").objectReferenceValue = lap;
            hudSerialized.FindProperty("checkpointText").objectReferenceValue = checkpoint;
            hudSerialized.FindProperty("nitroFill").objectReferenceValue = nitroFill;
            hudSerialized.FindProperty("raceManager").objectReferenceValue = raceManager;

            var countdown = MakeText(canvas.transform, "Race Countdown", "3", 150, new Vector2(0.5f, 0.53f), new Vector2(420f, 190f), TextAnchor.MiddleCenter);
            var finishPanel = CreateFinishPanel(canvas.transform, out var resultTitle, out var resultSummary, out var retryButton, out var garageButton);
            hudSerialized.FindProperty("countdownText").objectReferenceValue = countdown;
            hudSerialized.FindProperty("finishPanel").objectReferenceValue = finishPanel;
            hudSerialized.FindProperty("resultTitle").objectReferenceValue = resultTitle;
            hudSerialized.FindProperty("resultSummary").objectReferenceValue = resultSummary;
            hudSerialized.FindProperty("retryButton").objectReferenceValue = retryButton;
            hudSerialized.FindProperty("garageButton").objectReferenceValue = garageButton;
            var controlsRoot = new GameObject("Touch Controls", typeof(RectTransform));
            controlsRoot.transform.SetParent(canvas.transform, false);
            var controlsRect = controlsRoot.GetComponent<RectTransform>();
            controlsRect.anchorMin = Vector2.zero; controlsRect.anchorMax = Vector2.one;
            controlsRect.offsetMin = Vector2.zero; controlsRect.offsetMax = Vector2.zero;
            hudSerialized.FindProperty("drivingControlsRoot").objectReferenceValue = controlsRoot;
            hudSerialized.ApplyModifiedPropertiesWithoutUndo();

            var steerLeft = CreateDriveButton(controlsRoot.transform, input, "STEER LEFT", "◀", MobileDriveButton.DriveAction.SteerLeft, new Vector2(0.07f, 0.16f), new Vector2(132f, 132f));
            var steerRight = CreateDriveButton(controlsRoot.transform, input, "STEER RIGHT", "▶", MobileDriveButton.DriveAction.SteerRight, new Vector2(0.20f, 0.16f), new Vector2(132f, 132f));
            CreateDriveButton(controlsRoot.transform, input, "BRAKE", "BRAKE", MobileDriveButton.DriveAction.Brake, new Vector2(0.75f, 0.15f), new Vector2(150f, 100f));
            CreateDriveButton(controlsRoot.transform, input, "THROTTLE", "GO", MobileDriveButton.DriveAction.Throttle, new Vector2(0.88f, 0.16f), new Vector2(150f, 116f));
            CreateDriveButton(controlsRoot.transform, input, "HANDBRAKE", "DRIFT", MobileDriveButton.DriveAction.Handbrake, new Vector2(0.73f, 0.30f), new Vector2(128f, 78f));
            CreateDriveButton(controlsRoot.transform, input, "NITRO", "N₂O", MobileDriveButton.DriveAction.Nitro, new Vector2(0.88f, 0.31f), new Vector2(128f, 78f));
            var wheel = CreateSteeringWheel(controlsRoot.transform, input);
            controlsRoot.AddComponent<MobileControlLayout>().Configure(steerLeft, steerRight, wheel);
        }

        private static GameObject CreateFinishPanel(Transform parent, out Text title, out Text summary, out Button retry, out Button garage)
        {
            var overlay = new GameObject("Race Results Overlay", typeof(RectTransform), typeof(Image));
            overlay.transform.SetParent(parent, false);
            var rect = overlay.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero; rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero; rect.offsetMax = Vector2.zero;
            overlay.GetComponent<Image>().color = new Color(0.015f, 0.03f, 0.055f, 0.94f);

            var card = MakeImage(overlay.transform, "Race Results Card", new Color(0.035f, 0.075f, 0.12f, 1f), new Vector2(0.5f, 0.5f), new Vector2(840f, 570f));
            title = MakeText(card.transform, "Results Title", "RACE COMPLETE · 1ST PLACE", 48, new Vector2(0.5f, 0.84f), new Vector2(760f, 90f), TextAnchor.MiddleCenter);
            summary = MakeText(card.transform, "Results Summary", "TIME  00:00.000\nREWARD  8,000 CREDITS  ·  1,200 XP", 28, new Vector2(0.5f, 0.53f), new Vector2(720f, 230f), TextAnchor.MiddleCenter);
            retry = MakeResultButton(card.transform, "Retry Race", "RACE AGAIN", new Vector2(0.34f, 0.17f));
            garage = MakeResultButton(card.transform, "Return to Garage", "GARAGE", new Vector2(0.67f, 0.17f));
            overlay.SetActive(false);
            return overlay;
        }

        private static Button MakeResultButton(Transform parent, string name, string label, Vector2 anchor)
        {
            var obj = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
            obj.transform.SetParent(parent, false);
            var rect = obj.GetComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = anchor;
            rect.sizeDelta = new Vector2(250f, 74f);
            obj.GetComponent<Image>().color = new Color(0.02f, 0.58f, 0.9f, 1f);
            MakeText(obj.transform, name + " Label", label, 25, new Vector2(0.5f, 0.5f), new Vector2(240f, 64f), TextAnchor.MiddleCenter);
            return obj.GetComponent<Button>();
        }

        private static GameObject CreateDriveButton(Transform parent, MobileVehicleInput input, string name, string label,
            MobileDriveButton.DriveAction action, Vector2 anchor, Vector2 size)
        {
            var obj = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(MobileDriveButton));
            obj.transform.SetParent(parent, false);
            var rect = obj.GetComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = anchor;
            rect.sizeDelta = size;
            obj.GetComponent<Image>().color = action == MobileDriveButton.DriveAction.Throttle
                ? new Color(0.02f, 0.52f, 0.88f, 0.72f) : new Color(0.035f, 0.09f, 0.15f, 0.72f);
            var serialized = new SerializedObject(obj.GetComponent<MobileDriveButton>());
            serialized.FindProperty("input").objectReferenceValue = input;
            serialized.FindProperty("action").enumValueIndex = (int)action;
            serialized.ApplyModifiedPropertiesWithoutUndo();
            MakeText(obj.transform, name + " Label", label, 28, new Vector2(0.5f, 0.5f), new Vector2(size.x - 10f, size.y - 10f), TextAnchor.MiddleCenter);
            return obj;
        }

        private static GameObject CreateSteeringWheel(Transform parent, MobileVehicleInput input)
        {
            var wheel = new GameObject("Touch Steering Wheel", typeof(RectTransform), typeof(Image), typeof(MobileSteeringWheel));
            wheel.transform.SetParent(parent, false);
            var rect = wheel.GetComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = new Vector2(0.16f, 0.18f);
            rect.sizeDelta = new Vector2(190f, 190f);
            wheel.GetComponent<Image>().color = new Color(0.04f, 0.18f, 0.28f, 0.88f);
            var text = MakeText(wheel.transform, "Steering Wheel Mark", "◉", 104, new Vector2(0.5f, 0.5f), new Vector2(175f, 175f), TextAnchor.MiddleCenter);
            text.raycastTarget = false;
            wheel.GetComponent<MobileSteeringWheel>().Configure(input);
            return wheel;
        }

        private static void CreateMenuButton(Transform parent)
        {
            var obj = new GameObject("Return to Garage", typeof(RectTransform), typeof(Image), typeof(Button), typeof(DriveSceneLaunchButton));
            obj.transform.SetParent(parent, false);
            var rect = obj.GetComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = new Vector2(0.965f, 0.93f);
            rect.sizeDelta = new Vector2(116f, 54f);
            obj.GetComponent<Image>().color = new Color(0.025f, 0.08f, 0.14f, 0.88f);
            var launcher = new SerializedObject(obj.GetComponent<DriveSceneLaunchButton>());
            launcher.FindProperty("sceneName").stringValue = "01_MainMenu";
            launcher.ApplyModifiedPropertiesWithoutUndo();
            MakeText(obj.transform, "Garage Label", "GARAGE", 18, new Vector2(0.5f, 0.5f), new Vector2(108f, 48f), TextAnchor.MiddleCenter);
        }

        private static void CreateLighting()
        {
            var lightObject = new GameObject("Moonlight", typeof(Light));
            var light = lightObject.GetComponent<Light>();
            light.type = LightType.Directional;
            light.color = new Color(0.48f, 0.68f, 1f);
            light.intensity = 0.45f;
            lightObject.transform.rotation = Quaternion.Euler(35f, -30f, 0f);
            RenderSettings.ambientLight = new Color(0.12f, 0.18f, 0.28f);
        }

        private static void EnsureEventSystem()
        {
            if (Object.FindObjectOfType<EventSystem>() == null)
                new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        }

        private static GameObject Cube(string name, Vector3 position, Vector3 scale, Material material, Transform parent)
        {
            var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.name = name;
            obj.transform.SetParent(parent, false);
            obj.transform.position = position;
            obj.transform.localScale = scale;
            if (material != null) obj.GetComponent<Renderer>().sharedMaterial = material;
            return obj;
        }

        private static Material Material(string name, Color color, float metallic, Color? emission = null)
        {
            if (MaterialCache.TryGetValue(name, out var cached) && cached != null) return cached;
            var path = MaterialFolder + "/" + name.Replace(" ", "_") + ".mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                var shader = Shader.Find("Standard");
                if (shader == null) shader = Shader.Find("Universal Render Pipeline/Lit");
                material = new Material(shader) { name = name };
                AssetDatabase.CreateAsset(material, path);
            }
            material.color = color;
            material.metallic = metallic;
            material.smoothness = 0.65f;
            if (emission.HasValue)
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", emission.Value * 1.8f);
            }
            MaterialCache[name] = material;
            return material;
        }

        private static Text MakeText(Transform parent, string name, string value, int size, Vector2 anchor, Vector2 dimensions, TextAnchor alignment)
        {
            var obj = new GameObject(name, typeof(RectTransform), typeof(Text));
            obj.transform.SetParent(parent, false);
            var rect = obj.GetComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = anchor;
            rect.sizeDelta = dimensions;
            var text = obj.GetComponent<Text>();
            text.text = value;
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = size;
            text.fontStyle = FontStyle.Bold;
            text.alignment = alignment;
            text.color = Color.white;
            text.raycastTarget = false;
            return text;
        }

        private static Image MakeImage(Transform parent, string name, Color color, Vector2 anchor, Vector2 dimensions)
        {
            var obj = new GameObject(name, typeof(RectTransform), typeof(Image));
            obj.transform.SetParent(parent, false);
            var rect = obj.GetComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = anchor;
            rect.sizeDelta = dimensions;
            var image = obj.GetComponent<Image>();
            image.color = color;
            return image;
        }

        private static void EnsureFolder(string path)
        {
            var segments = path.Split('/');
            var current = segments[0];
            for (var i = 1; i < segments.Length; i++)
            {
                var next = current + "/" + segments[i];
                if (!AssetDatabase.IsValidFolder(next)) AssetDatabase.CreateFolder(current, segments[i]);
                current = next;
            }
        }

        private static void AddBuildScene(string path)
        {
            var scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
            foreach (var scene in scenes) if (scene.path == path) return;
            scenes.Add(new EditorBuildSettingsScene(path, true));
            EditorBuildSettings.scenes = scenes.ToArray();
        }
    }
}
#endif
