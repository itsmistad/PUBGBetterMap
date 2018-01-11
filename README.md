# PUBGBetterMap
A networking, sketchable overlay that will add markers from pubgmap.io to the in-game experience.

# Limitations
The overlay does not allow click-through events, preventing users from controlling background controls while the overlay is open. This means that you may not be able to control any part of the game using the mouse/joystick while the map overlay is visible. The overlay button uses a Windows API hook to create a global hotkey. It currently overrides tilde (`/~). Any functionality normally bound to this key will be overriden when the program is opened.

# As of 11th of January
The tool
- does not have network capability,
- does not have Lerp smoothing like the real map does,
- does not have map size expansion when zooming in like the real map does,
- currently uses ShatterNL's map as a test.

# Demonstration
[![01-11-2018_200277.mp4](https://cdn.mistad.net/01-11-2018_621957.jpg)](https://cdn.mistad.net/01-11-2018_200277.mp4)
