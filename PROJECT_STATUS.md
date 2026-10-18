# Street Velocity — Project Status

## COMPLETED
- Phase 1 web screens and responsive layout for splash, menu, career, events, cars, garage, customization, upgrades, race HUD, results, rewards, achievements, settings, controls, minimap, weather, and city environments.
- Phase 1 Unity menu generator with 22 navigable screen concepts, reference themed artwork, and screen specific info cards.
- Phase 2 web driving view with animated perspective road, keyboard and touch steering, throttle, braking, nitro, speed display, and local race rewards.
- Phase 2 Unity source for arcade vehicle physics, nitro, handbrake, platform input, follow camera, race HUD, touch controls, and a generated neon test street.
- Separate `Web_Game/` and `Mobile_Game/` folders with original city and car art.

## PHASE 1 AND 2 STATUS
- Phase 1 and Phase 2 source work is complete for the web and Unity mobile variants.
- Unity scene generation and device validation require Unity Editor; this workspace currently contains the Unity Assets source rather than generated `.unity` scenes or a full Unity project configuration.
- The Unity driving test scene uses primitive car and city geometry. Final 3D vehicle models and production track art are later polish work.

## REMAINING
- Phase 3: checkpoint race lifecycle, AI opponents, traffic and police behavior, full results and progression wiring.
- Phase 4: garage economy, saved native progression, full car/environment art, weather behavior, audio, optimization and device QA.
- Unity Editor compilation, scene generation, and device review.

## WEB AND MOBILE
- Run `python3 -m http.server 8000` from the workspace and open `http://localhost:8000/Web_Game/`.
- The browser version is responsive and installable on mobile. See `WEB_BUILD.md` for hosting and Unity target notes.
