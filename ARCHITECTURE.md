# Architecture

## Four implementation phases

1. **Foundation and screens** — folder conventions, bootstrap, state, services, scene routing, screen navigation, shared UI contracts.
2. **Driving** — configurable vehicle data, input abstraction, arcade vehicle behavior, camera and HUD integration.
3. **Racing and progression** — checkpoints/laps, race lifecycle/results, wallet/rewards, player profile and local save, career events.
4. **Expansion and polish** — AI, traffic, police, garage upgrades/customization, weather/audio, achievements/settings, optimization and QA.

## Dependency direction

`UI -> feature-facing interfaces -> feature services -> Core contracts`.
Core does not depend on UI or concrete gameplay. Feature configuration belongs in ScriptableObjects. Scenes are presentation/content containers; systems communicate through small interfaces and events rather than looking each other up globally.

## Adding a screen

1. Add a scene or screen prefab under `Mobile_Game/Assets/_StreetVelocity/`.
2. Implement `IUIScreen` in a focused controller under `Scripts/UI/Screens/`.
3. Register the screen key and factory with `ScreenRouter` during scene composition.
4. Navigate with `IScreenNavigator.Show(screenId)`; do not load scenes from button handlers.

## Adding a feature

Create a feature folder in `Scripts/`, define configuration as a ScriptableObject, keep the runtime service independent from MonoBehaviour UI, and connect it in the bootstrap/composition root. Avoid a new global manager unless it owns a real application-wide service.

## Namespaces

Use `StreetVelocity.Core`, `StreetVelocity.UI`, `StreetVelocity.Vehicle`, `StreetVelocity.Racing`, `StreetVelocity.Progression`, and feature-specific namespaces. Keep one public type per file.
