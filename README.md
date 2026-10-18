# Street Velocity — Mobile Racing Game

Street Velocity is a neon night racer for landscape Android/iOS, with a separately maintained responsive browser edition. Phase 1 provides 22 reference-inspired screens and career/garage flows; Phase 2 adds arcade driving and mobile touch, wheel, and tilt controls; Phase 3 adds five race event rules, results, rewards, and local progression. The web edition has its own race rendering, including rival placement, traffic hazards, drift scoring, and time trial targets.

## Game screenshots

These are captures from the browser edition. Unity screenshots require opening the generated scenes in Unity Editor.

| Desktop game | Phone layout |
| --- | --- |
| ![Street Velocity desktop game](Web_Game/screenshots/web-home.png) | ![Street Velocity phone game](Web_Game/screenshots/mobile-browser.png) |

Career map: ![Street Velocity career map](Web_Game/screenshots/web-career.png)

## Unity Android/iOS setup

Follow [`UNITY_SETUP_CHECKLIST.md`](UNITY_SETUP_CHECKLIST.md) to generate scenes and play. Unity 2022.3 LTS or newer is required. The generated native prototype includes sprint, a three-stage circuit route, drift scoring, a 15-second time trial, moving rivals, traffic, and police pursuit actors. Unity's circuit currently uses a straight generated street; rival placement rules, police capture conditions, production 3D assets, and device QA remain future work. Unity Editor compilation and device builds have not been run in this workspace.

## Web edition

The browser game is on [`main`](https://github.com/haroondhanyal/Street-Velocity-Unity-Web-Mobile-Racing/tree/main). Run `python3 -m http.server 8000` from the repository root and open `http://localhost:8000/Web_Game/`.

Browser localStorage and Unity PlayerPrefs saves are separate and do not sync.
