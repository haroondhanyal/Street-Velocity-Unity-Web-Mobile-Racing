# Street Velocity web and mobile build

The playable browser edition is in `Web_Game/`. It uses a responsive browser layout, supports keyboard and touch driving, saves the local garage and credits in browser storage, and can be installed as a landscape PWA.

## Run locally

Serve the workspace over HTTP so the service worker can install:

```sh
python3 -m http.server 8000
```

Open `http://localhost:8000/Web_Game/`. On a phone on the same network, open the computer's local IP at port `8000`; install it from the browser menu. A static host can publish the contents of `Web_Game/` directly.

## Unity targets

The separate native mobile source is under `Mobile_Game/Assets/_StreetVelocity/`. Add that `Assets` folder to a Unity 2022.3 LTS or newer project. Run **Street Velocity > Phase 1 > Setup Foundation**, then **Street Velocity > Phase 2 > Build Driving Test Scene**. The generated Unity screens are visual navigation mockups and the test scene has arcade driving. Phase 3 race events and AI still need development. Configure Android or iOS in Unity Build Settings for native builds.

The game artwork is copied into both projects. Browser game state is local to that device and is not synced with native builds.
