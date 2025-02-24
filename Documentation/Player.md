## Overview
The player scripts handles the players movement and animations.
## Features:
- **Player states and transitions**
- **Movement** using WASD
- **Surface handling**
- **Animation handling**
## Methods
`Start()`
Initializes the `Rigidbody2D` component.

`Update()`
Handles:
1. **Player Rotation**: Rotates the sprite based on velocity.
2. **Movement**: Moves the player using WASD keys.
3. **State Transitions**: Manages animations and state changes based on movement and collisions.

In update the inputs get checked to see if the player pressing any keys to move. If so the state is set to walking and the animation is started. then a force equal to the move speed is applied.

`OnCollisionStay2D(Collision2D collision)`
Handles surface interactions:
- Detects and sets `isGrounded` for regular ground.
- Applies effects for slime bridges (e.g., gravity reduction, sprite flipping).

`OnCollisionExit2D(Collision2D collision)`
Resets surface-specific behaviors when the player exits a surface:
- Resets `isGrounded` or `isOnSlime`.
- Restores gravity when leaving slime bridges.

## Player States

|State|Description|
|---|---|
|Idle|Default state when the player is stationary.|
|Walking|Active state when the player moves using WASD keys.|
|Falling|Triggered when the player is airborne and not on a surface.|