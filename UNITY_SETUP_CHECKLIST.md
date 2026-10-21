# Unity Setup Checklist

Native Android/iOS source lives in [`Mobile_Game/Assets/`](Mobile_Game/Assets/). The browser edition is separate in [`Web_Game/`](Web_Game/).

## Generate and play the Unity scenes

1. Open a Unity 2022.3 LTS or newer project with the built-in UI and Input System packages available. Copy `Mobile_Game/Assets/` into the Unity project and let Unity import it.
2. Run **Street Velocity > Phase 1 > Setup Foundation**. This creates and registers the Boot and Main Menu scenes, sets landscape orientation, and builds the career, garage, and other screens inside the generated Main Menu. The generator rebuilds its Main Menu scene on each run.
3. Run **Street Velocity > Phase 2 > Build Driving Test Scene**. This creates `09_DriveTest`, a generated neon city with cross streets, crosswalks, sidewalks, bus stops, moving two-way traffic and walking pedestrians, plus the selected-car setup, chase camera, race manager, HUD, touch controls, weather controller, and checkpoint triggers.
4. Open `Assets/_StreetVelocity/Scenes/00_Boot.unity` and press Play. Navigate Home → Career → Events, select a car/event/paint and choose the race grid. The generated scenes are source outputs and are not checked into this repository.
5. A three-second countdown gates driving input. Sprint and circuit spawn four moving rivals; non-police events also spawn moving traffic, while Police Escape spawns two pursuing units. Traffic and police contact slow the player; staying near police for eight seconds causes a bust. Sprint/Circuit HUD shows live rival position and the result reward uses finishing place. Sprint, circuit, drift, time trial, and police escape use separate race rules. Finish to see the race time, best time, result, credits, XP, level, and wallet. Retry reloads the driving scene; Garage returns to the menu.

## Mobile driving controls

- Touch buttons: left/right steering, throttle, brake, handbrake, and nitro.
- Steering wheel: drag the on-screen wheel to steer.
- Tilt: uses the device accelerometer on mobile. Choose the control mode in the Controls screen.
- Sensitivity and inverted steering are saved locally. Keyboard fallback in the Unity Editor uses W/A/S/D or arrows, Space to brake, Left Shift for handbrake, and N for nitro.

## Events currently generated

- **Street Sprint:** one route stage, 8,000 credits and 1,200 XP for a win.
- **Circuit Race:** three ordered checkpoint stages, 12,000 credits and 1,800 XP for a win. The current prototype places these stages along one long straight generated road; it is not a closed-loop track. Four simple AI rivals spawn for Sprint and Circuit; their distance determines the HUD position and finish payout.
- **Drift Event:** complete the 3,000-point drift target; 9,500 credits and 1,500 XP for a win.
- **Time Trial:** complete the route within 15 seconds for first place and full rewards; a missed target awards reduced rewards.
- **Police Escape:** complete the route and record an escape; 14,000 credits and 2,200 XP for a win. The current prototype spawns two simple police pursuit cars; close pursuit builds pressure and an eight-second hold ends the event as a bust with a reduced payout.

The procedural rival, traffic, and police cars are prototype geometry and follow the straight test road. Rival place uses distance on this linear route; police pressure is a prototype proximity timer. All rewards, car/event/paint choices, control preferences, weather, achievements, and progression use PlayerPrefs on that device. Browser localStorage is separate; accounts do not sync.

## Android and iOS builds

Install the Android or iOS Build Support module in Unity Hub, choose the platform in Build Settings, and set the bundle identifier, signing, and device permissions. The project source and Editor generators are provided; Unity Editor compilation, scene generation, and device builds have not been run in this workspace.
