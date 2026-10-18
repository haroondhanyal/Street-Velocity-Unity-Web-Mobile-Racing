# Street Velocity — Web Game

A responsive neon racing game for desktop browsers and phone screens. Phase 1 includes 22 reference-inspired screens and saved garage/career state. Phase 2 provides keyboard and touch arcade driving. Phase 3 runs a countdown race against five paced rivals, tracks laps and checkpoint sectors, calculates a finish place, and awards credits and XP.

## Run

From the repository root:

```sh
python3 -m http.server 8000
```

Open `http://localhost:8000/Web_Game/`. On phones, install the landscape PWA from the browser menu. Browser progress is stored on that device.

## Driving

- Keyboard: WASD or arrows to steer/accelerate, Down/S or Space to brake, Shift to drift, N for nitro, Escape to pause.
- Touch: hold left/right, throttle, brake, drift, and nitro controls.

Finish every race by driving the displayed route distance; finishing place changes the credits and XP payout. Engine upgrades, daily rewards, paint, selected car, settings, level, achievements, and best time save to browser storage.

## Screenshots

Captured screenshots are in [`screenshots/`](screenshots/).
