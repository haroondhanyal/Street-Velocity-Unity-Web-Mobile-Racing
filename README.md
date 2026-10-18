# Street Velocity — Web Racing Game

Street Velocity is a neon night street racing game for desktop browsers and phone screens. Phase 1 builds the reference-inspired screens and career flow; Phase 2 adds arcade driving with keyboard and touch controls; Phase 3 adds five race formats: rival sprint/circuit, drift scoring, a 28-second time trial, police-style pursuit, and traffic hazards. Results award placement-based credits and XP, with daily rewards, engine upgrades, weather effects, and saved progression. Desktop and phone layouts use separate responsive styling.

## Screenshots

| Desktop game | Career map |
| --- | --- |
| ![Street Velocity web home screen](Web_Game/screenshots/web-home.png) | ![Street Velocity career map](Web_Game/screenshots/web-career.png) |

Phone layout:

![Street Velocity phone browser layout](Web_Game/screenshots/mobile-browser.png)

## Run locally

```sh
python3 -m http.server 8000
```

Open [http://localhost:8000/](http://localhost:8000/) or [http://localhost:8000/Web_Game/](http://localhost:8000/Web_Game/).

## Controls

- Desktop: WASD or arrow keys to steer and accelerate, Down or Space to brake, Shift to drift, and N for nitro.
- Touch: on-screen steering, throttle, brake, drift, and nitro controls.

Progress is saved locally in the browser. See [`Web_Game/README.md`](Web_Game/README.md) for race flow and storage details. For Unity Android/iOS source, switch to the [`mobile` branch](https://github.com/haroondhanyal/Street-Velocity-Unity-Web-Mobile-Racing/tree/mobile).
