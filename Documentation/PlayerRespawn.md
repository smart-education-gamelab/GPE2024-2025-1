The playerRespawn scripts handles respawn functionality. It allows the player to either respawn at the checkpoint or a fallback position based on the last grounded position.

## Features
- **Respawn System**: Respawns the player at a checkpoint or nearby their last grounded position.
- **Checkpoint Integration**: Supports designated respawn points.
- **Life Adjustment**: Decreases the player's life when they respawn.
## Methods
`Update()`
- Continuously checks if the player is grounded using the `Playermovement` script.
- Updates `lastPoint` with the player's position when grounded.

`Respawn()`
Handles the respawn process:
1. Decreases the player's life using the `PlayerLifeManager`.
2. Checks if a checkpoint is active:
    - If `usingCheckpoint` is true, sets the player's position to `checkpointPosition`.
    - Otherwise, moves the player slightly away from `lastPoint` to prevent immediate collisions:
        - If the player is left of the last point, respawns to the right.
        - If the player is right of the last point, respawns to the left.

### Setting a Checkpoint:
``` csharp
playerRespawn.usingCheckpoint = true;
playerRespawn.checkpointPosition = new Vector2(10, 5); // Set to the checkpoint's position
```
## Triggering a respawn:
``` csharp
playerRespawn.Respawn(); // Respawns the player
```