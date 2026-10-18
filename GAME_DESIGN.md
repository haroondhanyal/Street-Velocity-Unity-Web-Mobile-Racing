# Game Design

Street Velocity is an original mobile-first arcade street racing game with keyboard support during development. The core loop is: choose a race, select a car, drive through ordered checkpoints, earn credits and XP, improve the garage, and unlock further events.

## Four content phases

1. **Foundation** — boot, main menu, garage and career screen shells with a shared navigation pattern.
2. **Driving** — responsive arcade handling with accelerate, brake/reverse, steering, handbrake and nitro inputs.
3. **Race loop** — countdown, checkpoints/laps, finish/results, credits/XP and local progression.
4. **World expansion** — AI opponents, multiple tracks and race types, traffic/pursuits, upgrades, weather, achievements and mobile polish.

## Screen flow

Boot → Main Menu → Career/Event → Garage/Car Selection → Race → Results → Garage or next event. Quick Race can branch from Main Menu and reuse the same selection and race flow.

## Extensibility rule

Screens own presentation and user actions. They call feature interfaces; they do not contain vehicle physics, economy rules, persistence, or scene-loading logic. New events and cars should be data assets rather than edits to a central gameplay class.
