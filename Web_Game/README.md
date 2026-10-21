# Street Velocity — Web Game

A browser-first retro coastal street racing game for desktop and mobile. Street Velocity: Neon Coast uses an original sunset-and-neon city style inspired by 1980s coastal arcade racers. The Web Game is the primary playable version; it needs no Unity install or account. It includes career, garage, five event formats, keyboard and touch driving, race results, progression, and local saves.

## Run

From the repository root:

```sh
python3 -m http.server 8000
```

Open `http://localhost:8000/Web_Game/`. Play directly in the browser or install it from the browser menu as a PWA. Desktop keyboards and mobile touch controls are supported. Browser progress is stored on that device.

## Driving and events

- Keyboard: WASD or arrows to steer/accelerate, Down/S or Space to brake, Shift to drift, N for nitro, Escape to pause.
- Touch: hold left/right, throttle, brake, drift, and nitro controls; steering wheel mode supports dragging.
- Sprint and three-lap circuit races run against five paced rivals. Drift ends at 3,000 points. Time Trial awards full placement/rewards at or below 28 seconds and reduced rewards after. Police Escape uses flashing pursuing cars; sustained close pressure causes a bust, while steering away or outrunning the units lets you escape. Traffic mode adds moving vehicles that slow the player on collision and reduce the payout. Quick challenge buttons select their corresponding event rules.

Race results include time, best lap, payout, XP, drift score or traffic hits, and career progression. Engine upgrades, daily streak rewards, paint, selected car, graphics and steering preferences, weather, achievements, and best time save to browser storage. Rain and fog affect the race canvas; graphics quality and visual effect toggles change rendering. Tilt steering requires supported motion sensors and browser permission.

## Screenshots

Captured screenshots are in [`screenshots/`](screenshots/).
