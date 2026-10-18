# Game Design

Street Velocity is a neon-night arcade racer for desktop browsers and landscape Android/iOS devices. The core loop is to choose a car and event, drive a route, meet its event objective, collect credits and XP, then return to the garage or career map.

## Delivery phases

1. **Foundation and screens:** boot/splash, 22 reference-inspired screens, navigation, car/event/district choice, garage/customization, local career, daily rewards, achievements, upgrades, settings, and responsive web/mobile presentation.
2. **Driving:** accelerate, brake/reverse, steer, drift, nitro, chase camera, speed HUD, keyboard and touch input; mobile also supports wheel and tilt steering.
3. **Race loop:** countdown, event rules, timer, route/checkpoints, results, rewards, XP/level progression, best times, retry, and return flow.
4. **Production expansion:** closed-loop tracks, native opponents and police AI, traffic simulation, audio, production vehicle/world art, optimization, and device QA.

## Screen flow

Splash → Home → Career map → Event selection → Car/garage → Race start → Countdown → Race HUD → Results → Retry, garage, or career. Quick race can reuse the same race setup.

## Event rules

- **Browser:** five paced rivals and placement for standard races; three-lap circuit; 3,000-point drift objective; 28-second time trial target; police-style pursuit rivals; and traffic hazards that slow the car on collision. Six route sectors update the race HUD.
- **Unity:** sprint crosses four ordered gates once; circuit repeats three checkpoint stages on the generated long straight; drift ends at 3,000 points; time trial awards full result within 15 seconds and reduced rewards on failure; police escape completes the route. These modes share the generated prototype street and do not yet have native police AI, native traffic, or native opponents.
- Browser localStorage and Unity PlayerPrefs are independent local saves. There is no player account or cross-device synchronization.

## Implementation rule

Screens own presentation and player actions. Vehicle physics, event rules, economy, persistence, and scene loading stay in feature components. New event definitions, vehicle configurations, and tracks should be data assets.
