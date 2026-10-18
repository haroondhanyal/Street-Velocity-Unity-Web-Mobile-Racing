# Street Velocity — Unity Mobile Game

Android/iOS Unity source for the neon racing project. Phase 1 generates all 22 landscape navigation screens and local reward/upgrade actions. Phase 2 generates arcade vehicle physics, nitro/drift, chase camera, HUD, and touch controls. Phase 3 adds a countdown, ordered checkpoint gates, a race timer, a finish/results overlay, retry/garage actions, and PlayerPrefs rewards/progression.

## Generate and play

Use Unity 2022.3 LTS or newer:

1. Copy this folder's `Assets/` into the Unity project root.
2. After Unity compiles the scripts, run **Street Velocity > Phase 1 > Setup Foundation**.
3. Run **Street Velocity > Phase 2 > Build Driving Test Scene**.
4. Open `Assets/_StreetVelocity/Scenes/00_Boot.unity` and press Play. Go through Career → Event → Race Start → Start Race.
5. Drive through the four checkpoint gates in order. Finish results save credits, XP, player level, wins, and best time locally.

Use WASD/arrows, Space, Shift, and N in the Editor; Android/iOS uses the screen buttons. Install the platform build support in Unity Hub and set Android/iOS build settings for a device build.

The generated Unity race is one one-lap Downtown Sprint with primitive vehicle and city geometry. The source scene generator has not been run in Unity in this workspace. See [`UNITY_SETUP_CHECKLIST.md`](../UNITY_SETUP_CHECKLIST.md) for the detailed flow and current limits.
