# Street Velocity — Unity Mobile Game

Android/iOS Unity source for the neon racing project. Phase 1 generates 22 landscape reference-inspired screens with selectable cars, events, paint, controls, daily rewards, achievements, graphics/weather preferences, and engine upgrades. Phase 2 generates arcade vehicle physics, car-specific handling, nitro/drift, chase camera, touch buttons, a drag steering wheel, and tilt steering. Phase 3 adds countdowns and five event rules: sprint, three-stage circuit, 3,000-point drift, 15-second time trial, and police escape, with results and PlayerPrefs progression.

## Generate and play

Use Unity 2022.3 LTS or newer:

1. Copy this folder's `Assets/` into the Unity project root.
2. After Unity imports and compiles the scripts, run **Street Velocity > Phase 1 > Setup Foundation**.
3. Run **Street Velocity > Phase 2 > Build Driving Test Scene**.
4. Open `Assets/_StreetVelocity/Scenes/00_Boot.unity` and press Play. Go through Career → Event → Race Start → Start Race.
5. Choose a car, event, paint, and controls before starting. The race begins after a countdown. Sprint and police escape use four ordered gates; circuit has three sequential gate stages; drift completes at 3,000 points; time trial gets full rewards under 15 seconds and reduced rewards after.
6. Results save credits, XP, level, wins, best time, drift points, top speed, and police route escapes locally. Car and paint selection affect the generated car; car choice changes speed, acceleration, and steering.

Editor keyboard controls are WASD/arrows, Space to brake, Shift for handbrake, and N for nitro. On device, choose touch arrows, drag wheel, or accelerometer tilt in Controls. Sensitivity and inverted steering are saved. Install platform build support in Unity Hub and configure Android/iOS signing and device settings.

All five events currently share the generated long neon street. Sprint/Circuit have procedural moving rivals, non-police modes have moving civilian traffic, and Police Escape has two pursuing cars; contact slows the player. Circuit is not a closed loop, rivals do not calculate finishing order, and police cannot capture/fail the player. These prototype actors use primitive geometry; finished 3D assets and audio remain future work. Unity Editor compilation, scene generation, and device builds have not been run in this workspace. See [`UNITY_SETUP_CHECKLIST.md`](../UNITY_SETUP_CHECKLIST.md) for setup steps and scope.
