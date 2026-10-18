# Street Velocity web and mobile build

The browser edition in `Web_Game/` includes responsive desktop/phone screens, PWA install support, local profile progression, arcade driving, and a complete Phase 3 browser race loop.

## Run locally

```sh
python3 -m http.server 8000
```

Open `http://localhost:8000/Web_Game/`. On a phone on the same network, use the computer's local IP at port `8000` and install from the browser menu. A static host can publish the contents of `Web_Game/` directly.

## Unity mobile

The separate Android/iOS source is under `Mobile_Game/Assets/_StreetVelocity/`. Copy `Mobile_Game/Assets/` into Unity 2022.3 LTS or newer, then run **Street Velocity > Phase 1 > Setup Foundation** and **Street Velocity > Phase 2 > Build Driving Test Scene**. The generated test scene includes Phase 3 countdown, ordered checkpoints, race timer, finish results, local rewards, retry, and garage return. Native source has one one-lap sprint and primitive vehicle/city art; AI, production tracks/assets, and device QA are future work.

The browser and Unity builds keep separate local saves.
