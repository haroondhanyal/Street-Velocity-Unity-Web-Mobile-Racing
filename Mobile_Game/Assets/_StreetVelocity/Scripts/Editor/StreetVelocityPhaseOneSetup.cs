#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using StreetVelocity.Core;
using StreetVelocity.UI;
using StreetVelocity.UI.Screens;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace StreetVelocity.EditorTools
{
    /// <summary>Creates a clean Phase 1 scene skeleton and wires its navigable menu screens.</summary>
    public static class StreetVelocityPhaseOneSetup
    {
        private const string Root = "Assets/_StreetVelocity";
        private const string SceneFolder = Root + "/Scenes";
        private static readonly string[] SceneNames =
        {
            "00_Boot", "01_MainMenu"
        };

        [MenuItem("Street Velocity/Phase 1/Setup Foundation")]
        public static void Setup()
        {
            EnsureFolders();
            CreateBootScene();
            CreateMenuScene();
            var buildScenes = new List<EditorBuildSettingsScene>();
            foreach (var sceneName in SceneNames)
                buildScenes.Add(new EditorBuildSettingsScene($"{SceneFolder}/{sceneName}.unity", true));
            EditorBuildSettings.scenes = buildScenes.ToArray();
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
            EditorSceneManager.OpenScene($"{SceneFolder}/00_Boot.unity");
            AssetDatabase.SaveAssets();
            Debug.Log("Street Velocity Phase 1 setup complete. Open 00_Boot and press Play.");
        }

        private static void EnsureFolders()
        {
            string[] folders = { "Art", "Art/Cars", "Art/Environment", "Art/Environment/worlds", "Art/Materials", "Art/Textures", "Art/VFX", "Art/Lighting",
                "Audio", "Audio/Music", "Audio/Engine", "Audio/UI", "Audio/Environment", "Audio/Police",
                "Prefabs", "Prefabs/Cars", "Prefabs/AI", "Prefabs/Police", "Prefabs/Environment", "Prefabs/UI", "Prefabs/Race",
                "Scenes", "ScriptableObjects", "ScriptableObjects/Cars", "ScriptableObjects/Races", "ScriptableObjects/Tracks",
                "ScriptableObjects/Economy", "ScriptableObjects/AI", "ScriptableObjects/Upgrades",
                "Scripts", "Scripts/Core", "Scripts/Bootstrap", "Scripts/Vehicle", "Scripts/Input", "Scripts/Camera",
                "Scripts/Race", "Scripts/AI", "Scripts/Police", "Scripts/Garage", "Scripts/Customization", "Scripts/Economy",
                "Scripts/Career", "Scripts/World", "Scripts/UI", "Scripts/Audio", "Scripts/Save", "Scripts/Services",
                "Scripts/Optimization", "Scripts/Debug", "Scripts/Utilities", "Scripts/Editor", "Tests", "Tests/EditMode", "Tests/PlayMode" };
            foreach (var folder in folders)
            {
                var path = Root;
                foreach (var segment in folder.Split('/'))
                {
                    var next = path + "/" + segment;
                    if (!AssetDatabase.IsValidFolder(next)) AssetDatabase.CreateFolder(path, segment);
                    path = next;
                }
            }
            AssetDatabase.Refresh();
        }

        private static void CreateBootScene()
        {
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>($"{SceneFolder}/00_Boot.unity") != null) return;
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            var bootstrap = new GameObject("GameBootstrap");
            bootstrap.AddComponent<GameBootstrap>();
            EditorSceneManager.SaveScene(scene, $"{SceneFolder}/00_Boot.unity");
        }

        private static void CreateMenuScene()
        {
            // Rebuild the generated menu so screen, art, and selection updates reach existing Unity projects.
            var menuScenePath = $"{SceneFolder}/01_MainMenu.unity";
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(menuScenePath) != null) AssetDatabase.DeleteAsset(menuScenePath);
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            CreateCamera();
            var canvasObject = new GameObject("MainCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            var canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;
            CreateEventSystem();

            var routerObject = new GameObject("ScreenRouter");
            var router = routerObject.AddComponent<ScreenRouter>();
            var panels = new List<MonoBehaviour>();
            CreatePanel<MainMenuScreen>(canvas.transform, router, "splash", "STREET VELOCITY", "THE CITY NEVER SLEEPS", panels,
                new[] { ("TAP TO START", "home") });
            CreatePanel<MainMenuScreen>(canvas.transform, router, "home", "STREET VELOCITY", "Own the night. Build your legend across Neon City.", panels,
                new[] { ("PLAY CAREER", "career"), ("CARS", "cars"), ("SETTINGS", "settings") });
            CreatePanel<CareerMapScreen>(canvas.transform, router, "career", "CAREER MAP", "Downtown · Industrial Zone · Harbor · Mountain Pass · Neon District", panels,
                new[] { ("SELECT EVENT", "events"), ("GARAGE", "garage"), ("BACK TO HOME", "home") });
            CreatePanel<CareerMapScreen>(canvas.transform, router, "events", "EVENT SELECTION", "Street Sprint · Circuit Race · Drift Event · Time Trial · Police Escape", panels,
                new[] { ("RACE GRID", "race-start"), ("CAREER MAP", "career") });
            CreatePanel<GarageScreen>(canvas.transform, router, "cars", "CAR SELECTION", "Falcon R · Vortex GT · Specter RS · Titan XR · Nova X", panels,
                new[] { ("SELECTED CAR GARAGE", "garage"), ("CAREER", "career") });
            CreatePanel<GarageScreen>(canvas.transform, router, "garage", "GARAGE", "Your active ride · performance · collection", panels,
                new[] { ("CUSTOMIZE", "customize"), ("UPGRADES", "upgrades"), ("CAR SELECTION", "cars") });
            CreatePanel<GarageScreen>(canvas.transform, router, "customize", "CUSTOMIZATION", "Paint · Rims · Spoiler · Tint · Plate", panels,
                new[] { ("SAVE LOOK", "garage"), ("UPGRADES", "upgrades") });
            CreatePanel<GarageScreen>(canvas.transform, router, "upgrades", "UPGRADE SCREEN", "Install parts and improve the selected car's race performance", panels,
                new[] { ("ENGINE", "UPGRADE_PART_0"), ("TURBO", "UPGRADE_PART_1"), ("ECU", "UPGRADE_PART_2"), ("TRANSMISSION", "UPGRADE_PART_3"), ("TIRES", "UPGRADE_PART_4"), ("BRAKES", "UPGRADE_PART_5"), ("SUSPENSION", "UPGRADE_PART_6"), ("NITRO", "UPGRADE_PART_7"), ("GARAGE", "garage"), ("RACE GRID", "race-start") });
            CreatePanel<RaceResultsScreen>(canvas.transform, router, "race-start", "RACE START", "Downtown · Event grid · Weather · Mobile controls", panels,
                new[] { ("START RACE", "09_DriveTest"), ("WEATHER", "weather"), ("CONTROLS", "controls") });
            CreatePanel<RaceResultsScreen>(canvas.transform, router, "race", "IN-RACE HUD", "Position · Lap · Timer · Minimap · Steering · Nitro", panels,
                new[] { ("RACE RESULTS", "results"), ("DRIFT EVENT", "drift"), ("MINIMAP", "minimap") });
            CreatePanel<RaceResultsScreen>(canvas.transform, router, "drift", "DRIFTING", "Score attack · Chain combos · Earn style points", panels,
                new[] { ("RESULTS", "results"), ("RACE HUD", "race") });
            CreatePanel<RaceResultsScreen>(canvas.transform, router, "nitro", "NITRO BOOST", "Trigger boost · Manage your meter · Chase top speed", panels,
                new[] { ("RESULTS", "results"), ("RACE HUD", "race") });
            CreatePanel<RaceResultsScreen>(canvas.transform, router, "police", "POLICE CHASE", "Heat level · Pursuit · Escape the city patrol", panels,
                new[] { ("RESULTS", "results"), ("RACE HUD", "race") });
            CreatePanel<RaceResultsScreen>(canvas.transform, router, "traffic", "TRAFFIC SYSTEM", "City traffic · Avoid collisions · Find the racing line", panels,
                new[] { ("RESULTS", "results"), ("RACE HUD", "race") });
            CreatePanel<RaceResultsScreen>(canvas.transform, router, "results", "RACE RESULTS", "Finish position · Race time · Best lap · Credits · XP", panels,
                new[] { ("RETRY", "race-start"), ("CONTINUE CAREER", "career"), ("GARAGE", "garage") });
            CreatePanel<MainMenuScreen>(canvas.transform, router, "rewards", "DAILY REWARDS", "Come back every day for exclusive rewards.", panels,
                new[] { ("CLAIM", "home"), ("HOME", "home") });
            CreatePanel<MainMenuScreen>(canvas.transform, router, "achievements", "ACHIEVEMENTS", "First Victory · Drift King · Speed Demon · Escape Artist", panels,
                new[] { ("HOME", "home"), ("CAREER", "career") });
            CreatePanel<MainMenuScreen>(canvas.transform, router, "settings", "SETTINGS", "Graphics · Controls · Audio · Gameplay", panels,
                new[] { ("MOBILE CONTROLS", "controls"), ("HOME", "home") });
            CreatePanel<MainMenuScreen>(canvas.transform, router, "controls", "MOBILE CONTROLS", "Touch buttons · Steering wheel · Tilt control · Sensitivity", panels,
                new[] { ("SAVE CONTROLS", "settings"), ("RACE GRID", "race-start") });
            CreatePanel<CareerMapScreen>(canvas.transform, router, "minimap", "MINIMAP", "Player · AI racer · Traffic · Police · Checkpoints · Finish", panels,
                new[] { ("RESUME RACE", "race") });
            CreatePanel<CareerMapScreen>(canvas.transform, router, "weather", "WEATHER & ENVIRONMENT", "Clear day · Night · Rain · Fog", panels,
                new[] { ("BACK TO GRID", "race-start") });
            CreatePanel<CareerMapScreen>(canvas.transform, router, "worlds", "GAME WORLD ENVIRONMENTS", "Downtown · Industrial Zone · Harbor · Mountain Pass", panels,
                new[] { ("CAREER MAP", "career"), ("BACK TO HOME", "home") });
            ConfigureRouter(router, panels.ToArray(), "splash");
            EditorSceneManager.SaveScene(scene, menuScenePath);
        }

        private static void CreatePanel<T>(Transform parent, ScreenRouter router, string id, string heading, string subtitle,
            List<MonoBehaviour> registered, (string label, string target)[] buttons) where T : UIScreen
        {
            var panel = new GameObject(id + " Screen", typeof(RectTransform), typeof(Image), typeof(CanvasGroup), typeof(T));
            panel.transform.SetParent(parent, false);
            var rect = panel.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero; rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero; rect.offsetMax = Vector2.zero;
            panel.GetComponent<Image>().color = new Color(0.035f, 0.055f, 0.09f, 1f);
            SetSerializedString(panel.GetComponent<T>(), "screenId", id);
            registered.Add(panel.GetComponent<T>());
            var artwork = ScreenArtwork(id);
            if (artwork != null)
            {
                var artObject = new GameObject("Night City Artwork", typeof(RectTransform), typeof(RawImage));
                artObject.transform.SetParent(panel.transform, false);
                var artRect = artObject.GetComponent<RectTransform>();
                artRect.anchorMin = Vector2.zero; artRect.anchorMax = Vector2.one;
                artRect.offsetMin = Vector2.zero; artRect.offsetMax = Vector2.zero;
                var rawImage = artObject.GetComponent<RawImage>();
                rawImage.texture = artwork;
                rawImage.color = ScreenArtTint(id);
                rawImage.uvRect = ScreenArtCrop(id);
                rawImage.raycastTarget = false;
            }
            if (id == "home" || id == "cars" || id == "garage" || id == "customize")
            {
                var carTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(Root + "/Art/Cars/street-car.png");
                if (carTexture != null)
                {
                    var carObject = new GameObject("Featured Street Car", typeof(RectTransform), typeof(RawImage));
                    carObject.transform.SetParent(panel.transform, false);
                    var carRect = carObject.GetComponent<RectTransform>();
                    carRect.anchorMin = carRect.anchorMax = new Vector2(0.5f, 0.5f);
                    carRect.sizeDelta = new Vector2(700, 390);
                    carRect.anchoredPosition = new Vector2(640, -150);
                    var carImage = carObject.GetComponent<RawImage>();
                    carImage.texture = carTexture;
                    carImage.color = new Color(1f, 1f, 1f, 0.96f);
                    carImage.raycastTarget = false;
                }
            }
            AddScreenContent(panel.transform, id);
            if (id == "home")
            {
                var careerCard = panel.transform.Find("CAREER REP Card");
                var dailyCard = panel.transform.Find("DAILY REWARD Card");
                var overview = panel.AddComponent<CareerOverviewController>();
                var overviewSerialized = new SerializedObject(overview);
                overviewSerialized.FindProperty("careerSummary").objectReferenceValue = careerCard.GetComponentsInChildren<Text>(true)[1];
                overviewSerialized.FindProperty("dailySummary").objectReferenceValue = dailyCard.GetComponentsInChildren<Text>(true)[1];
                overviewSerialized.ApplyModifiedPropertiesWithoutUndo();
            }
            if (id == "achievements")
            {
                var victory = panel.transform.Find("FIRST VICTORY Card");
                var drift = panel.transform.Find("DRIFT KING Card");
                var speed = panel.transform.Find("SPEED DEMON Card");
                var achievements = panel.AddComponent<CareerAchievementController>();
                var achievementSerialized = new SerializedObject(achievements);
                achievementSerialized.FindProperty("victoryText").objectReferenceValue = victory.GetComponentsInChildren<Text>(true)[1];
                achievementSerialized.FindProperty("driftText").objectReferenceValue = drift.GetComponentsInChildren<Text>(true)[1];
                achievementSerialized.FindProperty("speedText").objectReferenceValue = speed.GetComponentsInChildren<Text>(true)[1];
                achievementSerialized.ApplyModifiedPropertiesWithoutUndo();
            }
            AddText(panel.transform, heading, 56, new Vector2(0, 235), new Vector2(900, 100), FontStyle.Bold);
            AddText(panel.transform, subtitle, 24, new Vector2(0, 145), new Vector2(950, 60), FontStyle.Normal);
            var y = -300f;
            foreach (var item in buttons)
            {
                var buttonObject = new GameObject(item.label + " Button", typeof(RectTransform), typeof(Image), typeof(Button));
                buttonObject.transform.SetParent(panel.transform, false);
                var buttonRect = buttonObject.GetComponent<RectTransform>();
                buttonRect.anchorMin = buttonRect.anchorMax = new Vector2(0.5f, 0.5f);
                buttonRect.sizeDelta = new Vector2(350, 64);
                buttonRect.anchoredPosition = new Vector2(0, y);
                if (id == "upgrades")
                {
                    if (item.target.StartsWith("UPGRADE_PART_"))
                    {
                        var part = int.Parse(item.target.Substring("UPGRADE_PART_".Length));
                        buttonRect.anchoredPosition = new Vector2(part % 2 == 0 ? -390f : 390f, 25f - (part / 2) * 98f);
                        buttonRect.sizeDelta = new Vector2(620f, 70f);
                    }
                    else buttonRect.anchoredPosition = new Vector2(0f, item.target == "garage" ? -390f : -472f);
                }
                buttonObject.GetComponent<Image>().color = new Color(0.05f, 0.67f, 0.95f, 1f);
                if (item.target == "09_DriveTest")
                {
                    buttonObject.AddComponent<DriveSceneLaunchButton>();
                }
                else if (item.target.StartsWith("UPGRADE_PART_"))
                {
                    var upgrade = buttonObject.AddComponent<UpgradePurchaseButton>();
                    var upgradeSerialized = new SerializedObject(upgrade);
                    upgradeSerialized.FindProperty("partIndex").intValue = int.Parse(item.target.Substring("UPGRADE_PART_".Length));
                    upgradeSerialized.ApplyModifiedPropertiesWithoutUndo();
                }
                else if (id == "rewards" && item.label == "CLAIM")
                {
                    buttonObject.AddComponent<DailyRewardClaimButton>();
                }
                else
                {
                    var nav = buttonObject.AddComponent<ScreenNavigationButton>();
                    var navSerialized = new SerializedObject(nav);
                    navSerialized.FindProperty("router").objectReferenceValue = router;
                    navSerialized.FindProperty("targetScreenId").stringValue = item.target;
                    navSerialized.ApplyModifiedPropertiesWithoutUndo();
                }
                AddText(buttonObject.transform, item.label, 22, Vector2.zero, new Vector2(330, 56), FontStyle.Bold);
                y -= 76f;
            }
        }

        private static void AddScreenContent(Transform panel, string id)
        {
            if (id == "cars")
            {
                CreateOptionCards(panel, CareerOptionType.Car,
                    new[] { "FALCON R · CLASS B", "VORTEX GT · CLASS A", "SPECTER RS · DRIFT", "TITAN XR · MUSCLE", "NOVA X · CLASS S" },
                    new[] { "Balanced street coupe", "High speed sport coupe", "Precision drift setup", "Heavy launch power", "Top tier hyper car" });
                return;
            }
            if (id == "events")
            {
                CreateOptionCards(panel, CareerOptionType.Event,
                    new[] { "STREET SPRINT", "CIRCUIT RACE", "DRIFT EVENT", "TIME TRIAL", "POLICE ESCAPE" },
                    new[] { "1 lap · 8,000 CR · 1,200 XP", "3 laps · 12,000 CR · 1,800 XP", "3,000 drift pts · 9,500 CR", "Beat 15 sec · 7,500 CR", "Escape route · 14,000 CR" });
                return;
            }
            if (id == "controls")
            {
                CreateOptionCards(panel, CareerOptionType.Control,
                    new[] { "TOUCH BUTTONS", "STEERING WHEEL", "TILT STEERING" },
                    new[] { "On-screen directional controls", "Use the on-screen steering controls", "Steer by tilting your phone" });
                CreateControlPreference(panel, ControlPreferenceAction.ToggleInvert, new Vector2(0.34f, 0.5f));
                CreateControlPreference(panel, ControlPreferenceAction.CycleSensitivity, new Vector2(0.66f, 0.5f));
                return;
            }
            if (id == "settings")
            {
                CreateOptionCards(panel, CareerOptionType.Graphics,
                    new[] { "LOW GRAPHICS", "MEDIUM GRAPHICS", "HIGH GRAPHICS" },
                    new[] { "Battery-friendly detail", "Balanced visual quality", "Full visual quality" });
                return;
            }
            if (id == "weather")
            {
                CreateOptionCards(panel, CareerOptionType.Weather,
                    new[] { "CLEAR DAY", "NIGHT", "RAIN", "FOG" },
                    new[] { "Bright sky and dry road", "Neon night lighting", "Wet road and rain", "Reduced visibility" });
                return;
            }
            if (id == "customize")
            {
                CreateOptionCards(panel, CareerOptionType.Paint,
                    new[] { "NEON RED", "ELECTRIC BLUE", "CYAN", "GOLD", "PEARL WHITE" },
                    new[] { "#D91E45", "#087EFF", "#21D5FF", "#F5B72A", "#F0F2F6" });
                return;
            }

            string[] titles;
            string[] details;
            switch (id)
            {
                case "home": titles = new[] { "CAREER REP", "DAILY REWARD" }; details = new[] { "PROFILE_CAREER", "PROFILE_DAILY" }; break;
                case "career": titles = new[] { "DOWNTOWN", "INDUSTRIAL ZONE", "HARBOR" }; details = new[] { "6 / 10 EVENTS  ·  ★★★", "5 / 10 EVENTS  ·  ★★☆", "3 / 10 EVENTS  ·  ★★☆" }; break;
                case "events": titles = new[] { "STREET SPRINT", "CIRCUIT RACE", "DRIFT EVENT" }; details = new[] { "5.8 KM  ·  REWARD 8,000 CR", "3 LAPS  ·  REWARD 12,000 CR", "STYLE SCORE  ·  REWARD 9,500 CR" }; break;
                case "cars": titles = new[] { "TOP SPEED", "ACCELERATION", "HANDLING" }; details = new[] { "320 KM/H  ·  CLASS B", "280 / 400", "260 / 400" }; break;
                case "garage": titles = new[] { "ENGINE  ·  LV 3", "TURBO  ·  LV 2", "BRAKES  ·  LV 3" }; details = new[] { "TOP SPEED +10", "ACCELERATION +15", "BRAKING +5" }; break;
                case "customize": titles = new[] { "PAINT", "RIMS", "SPOILER" }; details = new[] { "9 BODY COLORS  ·  METALLIC", "SPORT  ·  FORGED  ·  NEON", "STREET WING  ·  GT WING" }; break;
                case "upgrades": titles = new[] { "ENGINE", "TURBO", "TIRES" }; details = new[] { "LEVEL 1 → 2  ·  COST 15,000", "LEVEL 2  ·  BOOST +1.2 SEC", "LEVEL 2  ·  GRIP +6" }; break;
                case "race-start": titles = new[] { "GRID POSITION", "RACE CONDITIONS", "EVENT REWARD" }; details = new[] { "2 / 6  ·  DOWNTOWN", "NIGHT  ·  WET ASPHALT", "8,000 CREDITS  ·  1,200 XP" }; break;
                case "race": titles = new[] { "POSITION  2 / 6", "LAP  1 / 2", "CHECKPOINT  3 / 12" }; details = new[] { "TIME  00:42.365", "BEST LAP  01:05.230", "NITRO  78%" }; break;
                case "drift": titles = new[] { "DRIFT SCORE", "COMBO", "STYLE BONUS" }; details = new[] { "3,450 POINTS", "× 3.2 MULTIPLIER", "CLEAN LINE  +500" }; break;
                case "nitro": titles = new[] { "BOOST METER", "PERFECT NITRO", "TOP SPEED" }; details = new[] { "3.4 SEC CAPACITY", "TIMED ACTIVATION", "320 KM/H" }; break;
                case "police": titles = new[] { "HEAT LEVEL 3", "PURSUIT UNITS", "ESCAPE ROUTE" }; details = new[] { "PATROL CLOSING IN", "2 UNITS  ·  1 BLOCK BACK", "HARBOR TUNNEL  ·  1.2 KM" }; break;
                case "traffic": titles = new[] { "CITY TRAFFIC", "ROAD HAZARD", "NEAR MISS BONUS" }; details = new[] { "MEDIUM DENSITY", "BOX TRUCK  ·  RIGHT LANE", "+250 XP PER CLOSE PASS" }; break;
                case "results": titles = new[] { "RACE TIME", "BEST LAP", "REWARD" }; details = new[] { "02:18.463", "01:05.230", "12,000 CR  ·  1,800 XP" }; break;
                case "rewards": titles = new[] { "DAY 1", "DAY 2", "DAY 3 · READY" }; details = new[] { "1,000 CREDITS", "2,000 CREDITS", "CAR PART DROP" }; break;
                case "achievements": titles = new[] { "FIRST VICTORY", "DRIFT KING", "SPEED DEMON" }; details = new[] { "ACHIEVEMENT_VICTORY", "ACHIEVEMENT_DRIFT", "ACHIEVEMENT_SPEED" }; break;
                case "settings": titles = new[] { "GRAPHICS", "AUDIO", "GAMEPLAY" }; details = new[] { "HIGH  ·  60 FPS", "MUSIC 70%  ·  SFX 85%", "LANGUAGE EN  ·  VIBRATION ON" }; break;
                case "controls": titles = new[] { "TOUCH BUTTONS", "STEERING WHEEL", "TILT CONTROL" }; details = new[] { "LEFT / RIGHT  ·  THROTTLE", "SENSITIVITY 50%", "DEVICE MOTION" }; break;
                case "minimap": titles = new[] { "PLAYER", "CHECKPOINT 3", "FINISH" }; details = new[] { "YOU ARE HERE", "1.2 KM AHEAD", "4.4 KM TO GO" }; break;
                case "weather": titles = new[] { "CLEAR DAY", "NIGHT RAIN", "FOG" }; details = new[] { "DRY ROAD  ·  HIGH VISIBILITY", "WET ROAD  ·  NEON REFLECTIONS", "LOW VISIBILITY  ·  CAUTION" }; break;
                case "worlds": titles = new[] { "DOWNTOWN", "INDUSTRIAL ZONE", "MOUNTAIN PASS" }; details = new[] { "STREET SPRINTS  ·  6/10", "CIRCUIT RACES  ·  5/10", "TOUGE EVENTS  ·  2/10" }; break;
                default: titles = new[] { "NEON DISTRICT", "RACE EVENT", "DRIVER PROFILE" }; details = new[] { "NIGHT CITY  ·  SEASON 04", "STREET VELOCITY", "PLAYER 01  ·  LEVEL 15" }; break;
            }

            var carScreen = id == "home" || id == "cars" || id == "garage" || id == "customize";
            var gap = carScreen ? 350f : 420f;
            var startX = carScreen ? -480f : -gap;
            for (var i = 0; i < titles.Length; i++)
            {
                if (carScreen && i == 2) continue;
                var x = startX + i * gap;
                var heading = carScreen && i == 1 && titles.Length == 3 ? titles[1] + " / " + titles[2] : titles[i];
                var detail = carScreen && i == 1 && details.Length == 3 ? details[1] + "  ·  " + details[2] : details[i];
                CreateInfoCard(panel, heading, detail, new Vector2(x, -75f), new Vector2(carScreen ? 310f : 380f, 158f));
            }
        }

        private static void CreateOptionCards(Transform parent, CareerOptionType optionType, string[] labels, string[] details)
        {
            var startX = labels.Length == 3 ? -340f : labels.Length == 4 ? -510f : -680f;
            const float gap = 340f;
            for (var i = 0; i < labels.Length; i++)
            {
                var card = new GameObject(labels[i] + " Option", typeof(RectTransform), typeof(Image), typeof(Button), typeof(CareerOptionButton));
                card.transform.SetParent(parent, false);
                var rect = card.GetComponent<RectTransform>();
                rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
                rect.sizeDelta = new Vector2(310f, 172f);
                rect.anchoredPosition = new Vector2(startX + i * gap, -76f);
                card.GetComponent<Image>().color = new Color(0.025f, 0.085f, 0.14f, 0.96f);
                var detail = optionType == CareerOptionType.Paint ? "BODY COLOR  " + details[i] : details[i];
                AddText(card.transform, labels[i], 20, new Vector2(0f, 34f), new Vector2(292f, 64f), FontStyle.Bold);
                AddText(card.transform, detail, 16, new Vector2(0f, -35f), new Vector2(292f, 64f), FontStyle.Normal);

                var serialized = new SerializedObject(card.GetComponent<CareerOptionButton>());
                serialized.FindProperty("optionType").enumValueIndex = (int)optionType;
                serialized.FindProperty("optionIndex").intValue = i;
                serialized.FindProperty("optionLabel").stringValue = labels[i];
                if (optionType == CareerOptionType.Paint) serialized.FindProperty("paintHex").stringValue = details[i];
                serialized.ApplyModifiedPropertiesWithoutUndo();
            }
        }

        private static void CreateControlPreference(Transform parent, ControlPreferenceAction action, Vector2 anchor)
        {
            var button = new GameObject("Control Preference", typeof(RectTransform), typeof(Image), typeof(Button), typeof(ControlPreferenceButton));
            button.transform.SetParent(parent, false);
            var rect = button.GetComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = anchor;
            rect.sizeDelta = new Vector2(390f, 58f);
            rect.anchoredPosition = new Vector2(0f, -215f);
            button.GetComponent<Image>().color = new Color(0.025f, 0.085f, 0.14f, 0.96f);
            var preference = new SerializedObject(button.GetComponent<ControlPreferenceButton>());
            preference.FindProperty("action").enumValueIndex = (int)action;
            preference.ApplyModifiedPropertiesWithoutUndo();
            var label = action == ControlPreferenceAction.ToggleInvert ? "INVERT STEERING" : "SENSITIVITY";
            AddText(button.transform, label, 17, Vector2.zero, new Vector2(370f, 52f), FontStyle.Bold);
        }

        private static void CreateInfoCard(Transform parent, string heading, string detail, Vector2 position, Vector2 size)
        {
            var card = new GameObject(heading + " Card", typeof(RectTransform), typeof(Image));
            card.transform.SetParent(parent, false);
            var rect = card.GetComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = size;
            rect.anchoredPosition = position;
            card.GetComponent<Image>().color = new Color(0.025f, 0.07f, 0.12f, 0.88f);
            AddText(card.transform, heading, 23, new Vector2(0f, 28f), new Vector2(size.x - 18f, 44f), FontStyle.Bold);
            AddText(card.transform, detail, 16, new Vector2(0f, -24f), new Vector2(size.x - 24f, 44f), FontStyle.Normal);
        }

        private static Color ScreenArtTint(string id)
        {
            if (id == "race" || id == "drift" || id == "police") return new Color(1f, 0.76f, 0.84f, 0.76f);
            if (id == "garage" || id == "cars" || id == "customize" || id == "upgrades") return new Color(0.8f, 0.92f, 1f, 0.72f);
            if (id == "career" || id == "worlds" || id == "minimap") return new Color(0.78f, 1f, 0.92f, 0.78f);
            if (id == "rewards" || id == "results") return new Color(1f, 0.9f, 0.68f, 0.76f);
            if (id == "weather") return new Color(0.84f, 0.92f, 1f, 0.74f);
            return new Color(0.82f, 0.9f, 1f, 0.74f);
        }

        private static Texture2D ScreenArtwork(string id)
        {
            var file = "downtown-night.jpg";
            if (id == "garage" || id == "customize" || id == "upgrades") file = "industrial-zone.jpg";
            else if (id == "worlds" || id == "minimap") file = "mountain-pass.jpg";
            else if (id == "weather" || id == "events") file = "harbor-night.jpg";
            var districtArt = AssetDatabase.LoadAssetAtPath<Texture2D>($"{Root}/Art/Environment/worlds/{file}");
            return districtArt != null ? districtArt : AssetDatabase.LoadAssetAtPath<Texture2D>(Root + "/Art/Environment/neon-district.png");
        }

        private static Rect ScreenArtCrop(string id)
        {
            if (id == "garage" || id == "cars" || id == "customize" || id == "upgrades") return new Rect(0.18f, 0.04f, 0.68f, 0.86f);
            if (id == "career" || id == "worlds" || id == "minimap") return new Rect(0.02f, 0.08f, 0.96f, 0.78f);
            if (id == "race" || id == "drift" || id == "police" || id == "traffic") return new Rect(0.32f, 0.02f, 0.64f, 0.94f);
            return new Rect(0f, 0f, 1f, 1f);
        }

        private static void ConfigureRouter(ScreenRouter router, MonoBehaviour[] panels, string initialId)
        {
            var serialized = new SerializedObject(router);
            var array = serialized.FindProperty("screenComponents");
            array.arraySize = panels.Length;
            for (var i = 0; i < panels.Length; i++) array.GetArrayElementAtIndex(i).objectReferenceValue = panels[i];
            serialized.FindProperty("initialScreenId").stringValue = initialId;
            serialized.ApplyModifiedPropertiesWithoutUndo();

        }

        private static void SetSerializedString(Object target, string property, string value)
        {
            var serialized = new SerializedObject(target);
            serialized.FindProperty(property).stringValue = value;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void AddText(Transform parent, string value, int size, Vector2 position, Vector2 dimensions, FontStyle style)
        {
            var textObject = new GameObject("Text - " + value, typeof(RectTransform), typeof(Text));
            textObject.transform.SetParent(parent, false);
            var rect = textObject.GetComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = dimensions; rect.anchoredPosition = position;
            var text = textObject.GetComponent<Text>();
            text.text = value; text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = size; text.fontStyle = style; text.alignment = TextAnchor.MiddleCenter;
            text.color = Color.white; text.raycastTarget = false;
        }

        private static void CreateCamera()
        {
            var cameraObject = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener));
            cameraObject.tag = "MainCamera";
            cameraObject.transform.position = new Vector3(0, 0, -10);
        }

        private static void CreateEventSystem()
        {
            var eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        }

        private static void CreatePlaceholderScenes()
        {
            for (var i = 2; i < SceneNames.Length; i++)
            {
                if (AssetDatabase.LoadAssetAtPath<SceneAsset>($"{SceneFolder}/{SceneNames[i]}.unity") != null) continue;
                var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                CreateCamera();
                var canvasObject = new GameObject("PlaceholderCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
                canvasObject.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
                AddText(canvasObject.transform, SceneNames[i] + " scene shell", 42, Vector2.zero, new Vector2(1200, 160), FontStyle.Bold);
                EditorSceneManager.SaveScene(scene, $"{SceneFolder}/{SceneNames[i]}.unity");
            }
        }
    }
}
#endif
