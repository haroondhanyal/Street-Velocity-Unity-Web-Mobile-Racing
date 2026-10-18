# Unity Setup Checklist

The Phase 1 and Phase 2 native mobile Unity source lives in [`Mobile_Game/Assets/`](Mobile_Game/Assets/). The web edition is separate in [`Web_Game/`](Web_Game/); see [`WEB_BUILD.md`](WEB_BUILD.md) for browser launch steps.

- **Run setup**: after Unity compiles the scripts, choose `Street Velocity > Phase 1 > Setup Foundation`. This creates the scene shells, 22 navigable screen concepts in Main Menu, Build Settings entries, and Boot scene with `GameBootstrap`.
- **Play**: in your Unity project, open `Assets/_StreetVelocity/Scenes/00_Boot.unity` and press Play. The sample screens share generated city and car art and link into the driving scene.
- **Phase 2 driving scene**: choose `Street Velocity > Phase 2 > Build Driving Test Scene`. From the menu use Career → Event → Race Start → Start Race. This creates `09_DriveTest` with an arcade car, neon test road, follow camera, HUD, nitro, and touch controls.
- **Input**: keyboard uses WASD or arrows, Space to brake, Left Shift for handbrake, and N for nitro. On phones, use the on screen steering, throttle, brake, drift, and nitro controls. Enable Unity's legacy Input Manager for keyboard input.
- **Mobile**: setup selects landscape orientation. Configure Android/iOS API and signing details before device builds. For an immediately playable phone version, open the web edition on the device and install it from the browser menu.
- **Web**: Unity WebGL is a separate target and needs the WebGL Build Support module. The browser game is developed independently in `Web_Game/`.

The Phase 1 command creates placeholder world scenes and the 22-screen navigation UI. Phase 2 creates a playable test street and primitive vehicle; final vehicle models, a full race loop, AI, and production tracks are later work. The browser edition has its own arcade drive view and touch/keyboard controls. Browser saves do not sync to Unity builds.
