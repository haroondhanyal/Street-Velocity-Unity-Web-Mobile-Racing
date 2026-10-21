# Speed Velocity — complete three phase web game

A playable browser game set in Velocity Bay. The React menu and save profiles open a procedural Three.js city with driving controls, a five chapter story campaign, career progression and local saves.

## Run locally

```sh
npm install
npm run dev
```

Open the local URL printed by Vite, then select **PLAY GAME**. A chapter intro and character dialogue play before the first drive. Use **CONTINUE STORY** later to launch the next unfinished chapter.

To create the production bundle:

```sh
npm run build
npm run preview
```

## Three phases included

- **Phase 1 — Web game foundation:** React and TypeScript menus, profile slots, character selection, modes, missions, garage paint, map, settings, pause menu and local persistence.
- **Phase 2 — Playable 3D city:** bundled Three.js city with roads, buildings, beach, day/night lighting, traffic, pedestrians, a player character and a drivable sports car. Keyboard, touch and gamepad controls are supported.
- **Phase 3 — Story and career:** five connected story chapters, intro/dialogue/mission-complete screens, mission unlock order, credits, REP ranks, achievements, route records and synthesized audio feedback. The Career page shows per-profile campaign progress and best times.

## Controls

- **WASD / arrow keys:** walk and drive
- **E:** enter or leave the car
- **M:** city map
- **P:** mission list
- **F:** garage
- **C:** switch chase camera
- **R:** reset the car
- **Space:** handbrake / jump on foot
- **Escape:** pause or skip a story scene

The game saves careers in this browser's local storage; no account or backend is required. The 3D game bundles Three.js locally, so its driving scene does not need a CDN connection.
