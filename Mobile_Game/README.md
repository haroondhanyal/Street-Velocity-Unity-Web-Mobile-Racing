# Street Velocity — Unity Mobile Game

Android/iOS Unity source for the neon racing project. Phases 1–4 generate 22 landscape reference-inspired screens, arcade vehicle physics, touch/wheel/tilt controls, five race events, rewards and PlayerPrefs progression, eight purchasable performance parts per selected car, and prototype rivals, traffic, and police pursuit actors.

## Generate and play

Use Unity 2022.3 LTS or newer:

1. Copy this folder's `Assets/` into the Unity project root.
2. After Unity imports and compiles the scripts, run **Street Velocity > Phase 1 > Setup Foundation**.
3. Run **Street Velocity > Phase 2 > Build Driving Test Scene**.
4. Open `Assets/_StreetVelocity/Scenes/00_Boot.unity` and press Play. Go through Career → Event → Race Start → Start Race.
5. Choose a car, event, paint, and controls before starting. The race begins after a countdown. Sprint and police escape use four ordered gates; circuit has three sequential gate stages; drift completes at 3,000 points; time trial gets full rewards under 15 seconds and reduced rewards after.
6. Results save credits, XP, level, wins, best time, drift points, top speed, and police route escapes locally. Car and paint selection affect the generated car; car choice changes speed, acceleration, and steering.

Editor keyboard controls are WASD/arrows, Space to brake, Shift for handbrake, and N for nitro. On device, choose touch arrows, drag wheel, or accelerometer tilt in Controls. Sensitivity and inverted steering are saved. Install platform build support in Unity Hub and configure Android/iOS signing and device settings.

All five events share a generated night city with cross streets, crosswalks, sidewalks, bus stops, moving two-way traffic, and walking pedestrians. Sprint/Circuit also have rivals, while Police Escape adds pursuing cars. These street-life actors are lightweight generated models intended for mobile performance. Sprint/Circuit show live place and pay out by finish order. Part upgrades save per selected car and change speed, acceleration, grip, braking, steering, or nitro physics. Circuit remains a straight-road prototype. Unity Editor compilation, scene generation, and device builds have not been run in this workspace. See [`UNITY_SETUP_CHECKLIST.md`](../UNITY_SETUP_CHECKLIST.md) for setup steps and scope.
