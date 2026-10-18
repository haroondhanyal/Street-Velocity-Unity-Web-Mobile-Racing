# Street Velocity — Web Game

A responsive neon racing game for desktop browsers and phone screens. Phase 1 includes 22 reference-inspired screens, saved garage/career state, daily rewards, and persistent settings. Phase 2 provides keyboard and touch arcade driving. Phase 3 runs countdown races against five paced rivals, tracks laps and checkpoint sectors, includes a score-target drift event, calculates placements, and awards credits and XP.

## Run

From the repository root:

```sh
python3 -m http.server 8000
```

Open `http://localhost:8000/Web_Game/`. On phones, install the landscape PWA from the browser menu. Browser progress is stored on that device.

## Driving

- Keyboard: WASD or arrows to steer/accelerate, Down/S or Space to brake, Shift to drift, N for nitro, Escape to pause.
- Touch: hold left/right, throttle, brake, drift, and nitro controls.

Finish sprint, circuit, time trial, and pursuit events by driving the displayed route distance. Drift events finish when you earn 3,000 drift points. Finishing place changes the credits and XP payout. Engine upgrades, weekly daily rewards, paint, selected car, graphics and steering preferences, weather, level, achievements, and best time save to browser storage. Rain and fog draw into the race scene; graphics quality and visual effect toggles change canvas rendering. Tilt steering requires motion sensor support and browser permission.

## Screenshots

Captured screenshots are in [`screenshots/`](screenshots/).
