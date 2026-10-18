# Street Velocity — Web Game

A responsive neon racing game for desktop browsers and phone screens. Phase 1 includes 22 reference-inspired screens, saved garage/career state, daily rewards, and persistent settings. Phase 2 provides keyboard and touch arcade driving. Phase 3 adds countdown races, five event formats, route/checkpoint progress, results, placement rewards, XP, achievements, and local career saves.

## Run

From the repository root:

```sh
python3 -m http.server 8000
```

Open `http://localhost:8000/Web_Game/`. On phones, install the landscape PWA from the browser menu. Browser progress is stored on that device.

## Driving and events

- Keyboard: WASD or arrows to steer/accelerate, Down/S or Space to brake, Shift to drift, N for nitro, Escape to pause.
- Touch: hold left/right, throttle, brake, drift, and nitro controls; steering wheel mode supports dragging.
- Sprint and three-lap circuit races run against five paced rivals. Drift ends at 3,000 points. Time Trial awards full placement/rewards at or below 28 seconds and reduced rewards after. Police Escape renders flashing police-style rival cars. Traffic mode adds moving vehicles that slow the player on collision.

Race results include time, best lap, payout, XP, drift score or traffic hits, and career progression. Engine upgrades, daily streak rewards, paint, selected car, graphics and steering preferences, weather, achievements, and best time save to browser storage. Rain and fog affect the race canvas; graphics quality and visual effect toggles change rendering. Tilt steering requires supported motion sensors and browser permission.

## Screenshots

Captured screenshots are in [`screenshots/`](screenshots/).
