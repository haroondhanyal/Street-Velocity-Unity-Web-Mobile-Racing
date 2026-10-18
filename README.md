# Street Velocity — Mobile Racing Game

Street Velocity is a neon night street racer for landscape Android/iOS devices, with a separate responsive browser edition. Phase 1 creates the 22-screen menu, career, event, and garage flow; Phase 2 adds arcade driving and touch controls; Phase 3 adds countdowns, checkpoints, race results, rewards, and local progression. The browser version includes rival placement and a score-target drift challenge. Desktop and phone layouts follow the supplied neon city racing artwork.

## Game screenshots

These are real captures from the browser edition; Unity screenshots require opening the generated scenes in Unity Editor.

| Desktop game | Phone layout |
| --- | --- |
| ![Street Velocity desktop game](Web_Game/screenshots/web-home.png) | ![Street Velocity phone game](Web_Game/screenshots/mobile-browser.png) |

Career map: ![Street Velocity career map](Web_Game/screenshots/web-career.png)

## Unity Android/iOS setup

1. Open a Unity 2022.3 LTS or newer project.
2. Copy `Mobile_Game/Assets/` into the Unity project root.
3. Run **Street Velocity > Phase 1 > Setup Foundation**.
4. Run **Street Velocity > Phase 2 > Build Driving Test Scene**.
5. Open `Assets/_StreetVelocity/Scenes/00_Boot.unity` and press Play.
6. Choose Career → Event → Race Start → Start Race. Wait through the countdown and cross all four checkpoint gates in order.
7. Results show time, best time, credit/XP rewards, player level and wallet. Retry reloads the race; Garage returns to the menu.
8. Install Android or iOS Build Support and configure platform build settings for a device build.

Touch controls are included in the generated race scene. Keyboard controls are WASD/arrows, Space to brake/reverse, Shift to drift, and N for nitro. Unity scene generation and device builds require Unity Editor.

## Web edition

The browser game and its screenshots are on [`main`](https://github.com/haroondhanyal/Street-Velocity-Unity-Web-Mobile-Racing/tree/main). Run `python3 -m http.server 8000` from the repository root and open `http://localhost:8000/Web_Game/`.

## Current scope

Unity Phase 3 currently has one one-lap Downtown Sprint with a primitive coupe and generated neon street. Browser races have five paced rivals and event-specific lap lengths. Native AI, multiple production tracks, traffic/police behavior, production vehicle/world assets, audio, optimization, and device QA are future Phase 4 work. Browser localStorage and Unity PlayerPrefs saves do not sync.
