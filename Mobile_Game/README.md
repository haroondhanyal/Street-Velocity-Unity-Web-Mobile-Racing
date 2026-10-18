# Street Velocity — Unity Mobile Game

Street Velocity is a neon night racing game for Android and iOS. Phase 1 provides the 22-screen navigation and garage flow; Phase 2 adds arcade vehicle physics, nitro, drift handling, a follow camera, race HUD, and touch controls. Its screen composition is built for landscape phone play and shares the visual theme with the separate browser edition.

The Unity scenes are generated in the Editor from the included setup tools. The driving test scene currently uses a primitive vehicle and generated city geometry.

## Open in Unity

Create or open a Unity 2022.3 LTS project, then copy this folder's `Assets/` directory into its project root. After Unity compiles, choose **Street Velocity > Phase 1 > Setup Foundation**, then **Street Velocity > Phase 2 > Build Driving Test Scene**. Open `Assets/_StreetVelocity/Scenes/00_Boot.unity` and press Play. Select Career → Event → Race Start → Start Race to drive.

Desktop uses WASD/arrows, Space to brake, Left Shift for handbrake, and N for nitro. Android/iOS use the on screen controls.

For a device build, install Android or iOS Build Support in Unity Hub and configure the signing and platform settings. The setup script selects landscape orientation. Detailed notes are in the workspace [`UNITY_SETUP_CHECKLIST.md`](../UNITY_SETUP_CHECKLIST.md).

The browser edition's actual desktop and phone screenshots are in [`Web_Game/screenshots/`](../Web_Game/screenshots/) on the repository's `main` branch.
