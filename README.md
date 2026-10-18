# Street Velocity — Mobile Racing Game

Street Velocity is a neon night street racing game for Android and iOS, with a separate browser game for desktop and phone. Phase 1 provides the 22-screen mobile UI and garage/career navigation. Phase 2 adds arcade driving, nitro, drift handling, a follow camera, HUD, and touch controls. The mobile screen layout is designed for landscape play and follows the supplied neon racing art direction.

## Game screenshots

The screenshots below are captured from the playable browser edition. The Unity mobile scenes are generated from the Editor menu described below.

| Desktop overview | Phone layout |
| --- | --- |
| ![Street Velocity desktop game](Web_Game/screenshots/web-home.png) | ![Street Velocity phone game](Web_Game/screenshots/mobile-browser.png) |

Career map: ![Street Velocity career map](Web_Game/screenshots/web-career.png)

## Unity Android/iOS setup

1. Open a Unity 2022.3 LTS or newer project.
2. Copy `Mobile_Game/Assets/` into the Unity project root.
3. After the scripts compile, run **Street Velocity > Phase 1 > Setup Foundation**.
4. Run **Street Velocity > Phase 2 > Build Driving Test Scene**.
5. Open `Assets/_StreetVelocity/Scenes/00_Boot.unity`, press Play, then choose Career → Event → Race Start → Start Race.
6. Enable Android or iOS Build Support and configure the platform build settings.

Touch controls are included in the generated driving scene. Unity keyboard controls are WASD/arrows, Space to brake, Shift to drift, and N for nitro. Unity scene generation and device builds require Unity Editor.

## Web edition

The browser version is on the repository's [`main` branch](https://github.com/haroondhanyal/Street-Velocity-Unity-Web-Mobile-Racing/tree/main). Run `python3 -m http.server 8000` from the repository root and open `http://localhost:8000/Web_Game/`.

## Current scope

Phase 1 and Phase 2 source is included for both variants. Unity uses a primitive test car and generated neon track geometry. AI opponents, complete race events, and production vehicle/world assets are later phases.
