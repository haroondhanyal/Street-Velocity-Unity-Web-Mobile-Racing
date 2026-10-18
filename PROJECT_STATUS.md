# Street Velocity — Project Status

## Phase 1 — Foundation and screens

- **Web:** Responsive desktop/phone UI covers the 22 reference screens: splash, home, career map, event selection, car selection, garage, customization, upgrades, race start/HUD, drifting, nitro, police, traffic, results, daily reward, achievements, settings, mobile controls, minimap, weather, and world environments. It includes selectable districts/events/cars, saved customization and upgrades, daily streak rewards, achievements, graphics/control/weather preferences, and local profile persistence.
- **Unity mobile:** Editor generators create the 22 landscape navigation screens, themed screen artwork, selectable cars/events/paint, career overview, achievements, control modes, sensitivity/inversion, graphics quality, weather, daily rewards, and engine upgrade flow. PlayerPrefs saves selections and progression.

## Phase 2 — Driving and controls

- **Web:** Perspective road renderer, player vehicle, moving rivals/traffic, steering, throttle, brake/reverse, handbrake/drift, nitro, pause, keyboard, touch arrows, virtual wheel, and tilt input with saved sensitivity/inversion. Weather effects and graphics settings affect rendering.
- **Unity mobile:** Rigidbody arcade vehicle with car-specific speed/acceleration/steering, selected paint, nitro, drift scoring, chase camera, speed/gear/nitro HUD, touch buttons, drag steering wheel, accelerometer steering, and saved control preferences. Generated environment has clear/night/rain/fog lighting and rain particles.

## Phase 3 — Events, race rules, and progression

- **Web:** Countdown and six sector HUD, paced rivals and placement, sprint/circuit, drift score objective, 28-second time trial target, police-themed pursuit race, and traffic avoidance with collision penalties. Results award placement-based credits/XP, level up, best time, achievements, and retry/career actions.
- **Unity mobile:** Selectable sprint, three-stage circuit, 3,000-point drift challenge, 15-second time trial, and police escape route. It has countdown, ordered physical checkpoint gates, timer and drift HUD, success/failure results, retry/garage, locally saved rewards/XP/level/best time, speed and drift records, and achievement progress.

## Current prototype limits

- Unity's circuit stages sit along one generated straight road rather than a loop; its cars and city use generated primitive geometry. Unity police escape records a route finish but has no police pursuit AI, and there are no native rival racers or traffic simulation yet.
- Web police mode renders flashing police-style rivals and traffic mode has passable collision hazards; neither is network multiplayer. The game uses local browser/PlayerPrefs saves with no cross-device account sync.
- Unity Editor compilation, generated-scene review, Android/iOS builds, device input checks, audio, production 3D art, optimization, and hardware QA remain unverified or future production work.

## Run and generate

- Web: from the repository root run `python3 -m http.server 8000`, then open `http://localhost:8000/Web_Game/`.
- Unity: copy `Mobile_Game/Assets/` into Unity 2022.3 LTS or newer, run **Street Velocity > Phase 1 > Setup Foundation**, then **Street Velocity > Phase 2 > Build Driving Test Scene**. See [`UNITY_SETUP_CHECKLIST.md`](UNITY_SETUP_CHECKLIST.md).
