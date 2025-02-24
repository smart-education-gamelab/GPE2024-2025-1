## Overview
The `BreakableObject` script manages an object that can break apart dynamically and reset to it's original state.
## Features
- **Breakable Object Behavior**: Breaks an object into pieces.
- **Particle Effects**: Plays a particle effect when the object breaks.
- **Object Reset**: Resets the object to its original position and state.

## Methods
`Start()`
- Initializes the object's state:
    - Stores the initial positions of the object pieces.
    - Adjusts the particle system's shape to fit the object's size.

`Update()`
- Monitors the position of the object pieces:
    - If a piece moves below `y = -40`, stops its movement by setting its velocity and gravity scale to zero.

`BreakWall()`
Triggers the breaking behavior:
1. Ensures the object isn’t already broken (`isBroken` check).
2. Plays the particle effect.
3. Disables the object's collider.
4. Sets the pieces to `Dynamic` rigidbody mode.
5. Applies gravity and random force to each piece to simulate breaking.

`ResetWall()`
Resets the object to its original state:
1. Ensures the object is broken (`isBroken` check).
2. Resets `isBroken` to `false`.
3. Sets each piece to `Kinematic` mode.
4. Removes gravity and velocity from each piece.
5. Repositions each piece to its starting position.
## Triggering a wall break and resetting it
``` csharp
breakableObject.BreakWall(); // breaking the wall

breakableObject.ResetWall(); // resetting it
```