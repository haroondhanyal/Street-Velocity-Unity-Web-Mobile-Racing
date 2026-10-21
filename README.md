# Street Velocity — Web Racing Game

Street Velocity is a neon night racer with separate responsive web and landscape Android/iOS editions. Phases 1–4 include 22 reference-inspired screens, career and garage flows, mobile touch/wheel/tilt controls, five race events, rewards and local progression, eight purchasable performance parts per car, and prototype traffic, rivals, and police actors. The browser edition also has its own race rendering with drift scoring and time trial targets.

## New 3D prototype: Speed Velocity

The original `Web_Game/` remains the established arcade racer. A separate Three.js vertical slice in [`speed-velocity/`](speed-velocity/README.md) adds a walkable coastal city, a drivable car, checkpoints, map, and menus. Run it from the repository root with `python3 -m http.server 8001` and open [http://localhost:8001/speed-velocity/](http://localhost:8001/speed-velocity/).

## Screenshots

| Desktop game | Career map |
| --- | --- |
| ![Street Velocity web home screen](Web_Game/screenshots/web-home.png) | ![Street Velocity career map](Web_Game/screenshots/web-career.png) |

Phone layout:

![Street Velocity phone browser layout](Web_Game/screenshots/mobile-browser.png)

### Speed Velocity full app gallery

These screenshots cover the complete web app and its phone-sized responsive layout.

| App screen | Desktop | Phone |
| --- | --- | --- |
| Home | ![Home screen on desktop](speed-velocity/screenshots/01-home.png) | ![Home screen on phone](speed-velocity/screenshots/mobile-01-home.png) |
| Save slots | ![Save slots on desktop](speed-velocity/screenshots/02-save-slots.png) | ![Save slots on phone](speed-velocity/screenshots/mobile-02-save-slots.png) |
| Character | ![Character screen on desktop](speed-velocity/screenshots/03-character.png) | ![Character screen on phone](speed-velocity/screenshots/mobile-03-character.png) |
| Game modes | ![Game modes on desktop](speed-velocity/screenshots/04-modes.png) | ![Game modes on phone](speed-velocity/screenshots/mobile-04-modes.png) |
| Missions | ![Missions on desktop](speed-velocity/screenshots/05-missions.png) | ![Missions on phone](speed-velocity/screenshots/mobile-05-missions.png) |
| Career | ![Career screen on desktop](speed-velocity/screenshots/06-career.png) | ![Career screen on phone](speed-velocity/screenshots/mobile-06-career.png) |
| Garage | ![Garage on desktop](speed-velocity/screenshots/07-garage.png) | ![Garage on phone](speed-velocity/screenshots/mobile-07-garage.png) |
| City map | ![City map on desktop](speed-velocity/screenshots/08-city-map.png) | ![City map on phone](speed-velocity/screenshots/mobile-08-city-map.png) |
| Settings | ![Settings on desktop](speed-velocity/screenshots/09-settings.png) | ![Settings on phone](speed-velocity/screenshots/mobile-09-settings.png) |
| Pause menu | ![Pause menu on desktop](speed-velocity/screenshots/10-pause.png) | ![Pause menu on phone](speed-velocity/screenshots/mobile-10-pause.png) |
| Code and build | ![Code and build screen on desktop](speed-velocity/screenshots/11-code-build.png) | ![Code and build screen on phone](speed-velocity/screenshots/mobile-11-code-build.png) |
| Play / story intro | ![Play screen on desktop](speed-velocity/screenshots/12-play-game.png) | ![Play screen on phone](speed-velocity/screenshots/mobile-12-play-game.png) |

## Run locally

```sh
python3 -m http.server 8000
```

Open [http://localhost:8000/](http://localhost:8000/) or [http://localhost:8000/Web_Game/](http://localhost:8000/Web_Game/).

## Controls

- Desktop: WASD or arrow keys to steer and accelerate, Down or Space to brake, Shift to drift, and N for nitro.
- Touch: on-screen steering, throttle, brake, drift, and nitro controls.

Progress is saved locally in the browser. See [`Web_Game/README.md`](Web_Game/README.md) for race flow and storage details. For Unity Android/iOS source, switch to the [`mobile` branch](https://github.com/haroondhanyal/Street-Velocity-Unity-Web-Mobile-Racing/tree/mobile).
