# Game Design

Street Velocity is a neon-night arcade racer for desktop browsers and landscape Android/iOS devices. The core loop is to choose a car and event, drive a route, meet its event objective, collect credits and XP, then return to the garage or career map.

## Delivery phases

1. **Foundation and screens:** boot/splash, 22 reference-inspired screens, navigation, car/event/district choice, garage/customization, local career, daily rewards, achievements, upgrades, settings, and responsive web/mobile presentation.
2. **Driving:** accelerate, brake/reverse, steer, drift, nitro, chase camera, speed HUD, keyboard and touch input; mobile also supports wheel and tilt steering.
3. **Race loop:** countdown, event rules, timer, route/checkpoints, results, rewards, XP/level progression, best times, retry, and return flow.
4. **World expansion:** procedural native rivals, ambient traffic, and police pursuit actors now appear in the generated Unity test scene. The next production work is closed-loop tracks, smarter opponent racing lines, richer traffic behavior, audio, production vehicle/world art, optimization, and device QA.

## Screen flow

Splash → Home → Career map → Event selection → Car/garage → Race start → Countdown → Race HUD → Results → Retry, garage, or career. Quick race can reuse the same race setup.

## Event rules

- **Browser:** five paced rivals and placement for standard races; three-lap circuit; 3,000-point drift objective; 28-second time trial target; pursuing police with an eight-second close-pressure bust condition; and traffic hazards that slow the car and reduce payout. Six route sectors update the race HUD.
- **Unity:** sprint crosses four ordered gates once; circuit repeats three checkpoint stages on the generated long straight; drift ends at 3,000 points; time trial awards full result within 15 seconds and reduced rewards on failure; police escape completes the route. These modes share the generated prototype street. Unity now spawns procedural rivals for sprint/circuit, ambient traffic, and two police pursuit actors; collisions slow the player, rival distance determines position, and eight seconds of close police pressure causes a bust.
- Browser localStorage and Unity PlayerPrefs are independent local saves. There is no player account or cross-device synchronization.

## Implementation rule

Screens own presentation and player actions. Vehicle physics, event rules, economy, persistence, and scene loading stay in feature components. New event definitions, vehicle configurations, and tracks should be data assets.
