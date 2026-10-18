# Street Velocity — Web Racing Game

Street Velocity is a neon night street racing game built for desktop browsers and phone screens. Phase 1 provides the game screens and career flow; Phase 2 adds arcade driving with keyboard and touch controls. The visual direction follows the supplied city racing reference, with separate responsive layouts for desktop and mobile.

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

Progress is saved locally in the browser. For Unity Android/iOS source, switch to the [`mobile` branch](https://github.com/haroondhanyal/Street-Velocity-Unity-Web-Mobile-Racing/tree/mobile).
