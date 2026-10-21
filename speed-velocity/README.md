# Speed Velocity — complete three phase browser game

An original browser driving game with a React game menu and a code-built Three.js coastal city. The React foundation is in `phase-1/`; its `/play` screen launches the bundled 3D city. The standalone 3D version remains available from this folder.

## Screenshots

The phone image shows the responsive web layout at a mobile viewport size.

| Desktop web | Mobile web |
| --- | --- |
| ![Speed Velocity desktop home screen](screenshots/web-desktop.png) | ![Speed Velocity mobile web layout](screenshots/mobile-web.png) |

## Start the full game

```sh
cd phase-1
npm install
npm run dev
```

Open the local Vite address it prints. Select **PLAY GAME** to launch the story intro. The React app includes menus, profiles, mission selection, garage, settings, career progress and source browsing. The 3D game is bundled under `phase-1/public/drive/`, so it can be previewed and built as one site.

## Standalone 3D prototype

```sh
python3 -m http.server 8001
```

Open `http://localhost:8001/speed-velocity/` from the repository root (or `http://localhost:8001/` when run in this folder). The standalone page uses the same Three.js world and systems. Three.js is loaded from jsDelivr, so the browser needs internet access.

## Implemented

- Original tropical coastal city with a road grid, beach, art deco hotel, buildings, palms, street lights, traffic and pedestrians.
- Third-person walking and driving, car entry/exit, arcade steering, braking, collision response, camera orbit and touch controls.
- Five connected story chapters with character dialogue, chapter transitions, sequential mission unlocks, credit rewards, first-clear REP, rank progression, achievements and per-mission best times. Career data saves locally per profile.
- HUD, minimap, mission gates, waypoint GPS, paint shop, vehicle service, dynamic day/night settings and graphics presets.
- Generated original soundtrack-like ambience, synthesized engine tone and short mission/collision cues using Web Audio. Audio settings persist locally.
- React shell with profile slots, character choice, modes, missions, garage, map, settings, pause, real source viewer and embedded 3D game.

## Three phases included

1. **Phase 1 — menus, profiles, settings and local saves:** complete in `phase-1/`.
2. **Phase 2 — coastal 3D city and driving:** bundled Three.js world with keyboard, touch and gamepad controls.
3. **Phase 3 — story and career:** five connected chapters, dialogue and completion screens, mission unlocks, REP/ranks, achievements, route records and synthesized audio.

## Project layout

- `phase-1/src/App.tsx` — React screens and route flow.
- `phase-1/src/store.ts` — persistent local profiles and settings.
- `phase-1/src/App.tsx` — career dashboard, story launch flow and source viewer.
- `phase-1/public/drive/` — self-contained copy of the playable 3D world for the React app.
- `src/main.js` — Three.js game loop, UI panels, save state and career integration.
- `src/systems/CareerSystem.js` — ranks, achievement conditions, mission unlocks and records.
- `src/systems/AudioSystem.js` — original synthesized ambience, engine and feedback sounds.
- `src/game/` — world geometry, vehicle/person meshes and arcade physics.
- `public/assets/velocity-bay-cover.png` — generated original menu key art.

## Notes

The environment, car and pedestrians are original procedural meshes rather than production-ready detailed GLB assets. Driving physics are arcade-style. Local saves stay in this browser; this build has no online account, cloud sync or multiplayer backend.
