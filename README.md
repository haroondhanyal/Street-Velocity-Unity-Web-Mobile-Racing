# Street Velocity — Mobile Racing Game

Street Velocity is a neon night racer for landscape Android/iOS, with a separately maintained responsive browser edition. Phases 1–4 include 22 reference-inspired screens, career and garage flows, mobile touch/wheel/tilt controls, five race events, rewards and local progression, eight purchasable performance parts per car, and prototype traffic, rivals, and police actors. The browser edition also has its own race rendering with drift scoring and time trial targets.

## Game screenshots

These are captures from the browser edition. Unity screenshots require opening the generated scenes in Unity Editor.

| Desktop game | Phone layout |
| --- | --- |
| ![Street Velocity desktop game](Web_Game/screenshots/web-home.png) | ![Street Velocity phone game](Web_Game/screenshots/mobile-browser.png) |

Career map: ![Street Velocity career map](Web_Game/screenshots/web-career.png)

## Unity Android/iOS setup

Follow [`UNITY_SETUP_CHECKLIST.md`](UNITY_SETUP_CHECKLIST.md) to generate scenes and play. Unity 2022.3 LTS or newer is required. The generated native prototype includes sprint, a three-stage circuit route, drift scoring, a 15-second time trial, moving rivals, traffic, and police pursuit actors. Rivals affect place/rewards, and sustained police proximity can bust the player. Unity's circuit currently uses a straight generated street; advanced opponent driving, production 3D assets, and device QA remain future work. Unity Editor compilation and device builds have not been run in this workspace.

## Web edition

The browser game is on [`main`](https://github.com/haroondhanyal/Street-Velocity-Unity-Web-Mobile-Racing/tree/main). Run `python3 -m http.server 8000` from the repository root and open `http://localhost:8000/Web_Game/`.

Browser localStorage and Unity PlayerPrefs saves are separate and do not sync.
