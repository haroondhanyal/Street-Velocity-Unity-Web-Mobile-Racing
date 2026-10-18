# Street Velocity — Project Status

## Phase 1 — Foundation and screens

- Web: responsive desktop and phone layouts for all 22 reference screens, with neon city/car art, splash/menu navigation, career districts, event selection, car collection, garage, paint customization, upgrades, rewards, achievements, controls, settings, weather, minimap, and world selection.
- Web interactions: car/event selection, paint choice, progressive engine purchase, once-per-day weekly reward schedule, achievement tracking, persistent graphics/render options, control method and steering settings, weather selection with visible race effects, and browser-local profile persistence.
- Unity mobile: Editor generator creates the 22 landscape screen concepts and navigation flow. Daily reward claims, engine upgrade purchases, and career wallet/XP use PlayerPrefs.

## Phase 2 — Driving

- Web: animated perspective road, player car and rival traffic, speed/gear telemetry, steering, throttle, brake/reverse, handbrake/drift, nitro, pause, keyboard input, and touch controls.
- Unity mobile: Rigidbody arcade car, acceleration/braking, grip/drift, nitro meter, chase camera, speedometer, keyboard input, and touch HUD controls.

## Phase 3 — Race loop

- Web: three-second start countdown, race distance, lap and six-sector progress HUD, five moving rivals and placement, a drift score objective, finish/results screen, placement-based credits and XP, level progression, best-time saving, and retry/career flow.
- Unity mobile: countdown gates vehicle input, four ordered trigger checkpoints finish the one-lap Downtown Sprint, timer and checkpoint HUD update, results overlay offers retry or garage, and PlayerPrefs saves credits, XP, level, wins, and best time.

## Setup and limits

- Web runs from the repository root with `python3 -m http.server 8000`; open `http://localhost:8000/Web_Game/`.
- Unity source and Editor scene generators are in `Mobile_Game/Assets/`. Copy Assets into Unity 2022.3 LTS or newer, run **Street Velocity > Phase 1 > Setup Foundation**, then **Street Velocity > Phase 2 > Build Driving Test Scene**. Unity Editor compilation, scene generation, and device builds have not been run in this workspace.
- Unity Phase 3 currently ships one one-lap test event with primitive vehicle/city geometry. Unity AI opponents, more tracks/race definitions, traffic and police behavior, audio, production 3D art, optimization, and device QA are Phase 4 work.
- Web localStorage and Unity PlayerPrefs are separate local saves; there is no cross-device account sync.
