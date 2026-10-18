# Game Design

Street Velocity is an original neon-night arcade street racer for desktop browsers and landscape Android/iOS devices. The current core loop is: choose a car and event, start a race, clear its route checkpoints, finish, earn credits/XP, and return to the garage or career map.

## Delivery phases

1. **Foundation and screens** — boot/splash, main menu, all 22 reference screens, navigation, car/event selection, garage, local profile, daily reward, upgrade purchase, settings, and responsive layouts.
2. **Driving** — accelerate, brake/reverse, steering, handbrake drift, nitro, chase camera, speed/HUD, keyboard and touch controls.
3. **Race loop** — countdown, race timer, lap/checkpoint progress, rival placement in the browser edition, finish, result summary, rewards, XP/level progression, best-time record, retry and return flow.
4. **World expansion** — native AI racers, multiple Unity tracks and event definitions, traffic/pursuits, deeper upgrade/customization data, dynamic weather, audio, production vehicle art, optimization, and device QA.

## Screen flow

Splash → Home → Career map → Event selection → Car/garage → Race start → Countdown → Race HUD → Results → Retry, garage, or career. Quick race can reuse the same race setup.

## Race rules in the current build

- Browser races have five paced rivals and calculate finishing position from route progress. Each event defines its lap count and route length; six checkpoint sectors divide each lap.
- Unity's generated Downtown Sprint is a single-player one-lap event. It starts after a three-second countdown, accepts four ordered physical trigger gates, then shows results and saves credits, XP, level, wins, and best time locally.
- Web localStorage and Unity PlayerPrefs are independent. Neither edition currently synchronizes player accounts.

## Extensibility rule

Screens own presentation and player actions. Vehicle physics, race rules, economy, persistence, and scene loading stay in their feature components. New event definitions and vehicle configs should be data assets.
