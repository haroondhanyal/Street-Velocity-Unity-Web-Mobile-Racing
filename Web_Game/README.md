# Street Velocity — Web Game

This is the standalone browser version. It adapts its layout for desktop and phone screens, supports keyboard and touch controls, and stores progress locally in the browser.

Race controls: WASD or arrow keys to steer and accelerate, down/S or Space to brake, Left Shift to drift, and N for nitro. Touch users can steer, brake, accelerate, drift, and trigger nitro with the on screen buttons.

## Start

From the workspace root, run:

```sh
python3 -m http.server 8000
```

Open `http://localhost:8000/Web_Game/`. On a phone on the same Wi-Fi, use the computer's local IP address with port `8000`, then install the game from the browser menu. More details are in the workspace [`WEB_BUILD.md`](../WEB_BUILD.md).
