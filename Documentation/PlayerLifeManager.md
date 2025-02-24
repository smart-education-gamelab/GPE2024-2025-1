## Overview
The PlayerLifeManager script handles the dynamic creation of health loss and accounts for the losing of life by updating the display.
## Features
- **Dynamic Health UI**: Displays health as a series of icons.
- **Health Management**: Adjusts the player's health and updates the UI accordingly.
- **Game Over Trigger**

## Methods

`Start()`
Initializes the health UI by:
1. Creating health icons based on the initial `playerHP`.
2. Positioning each icon horizontally with consistent spacing.

`Update()`
Monitors the player's health:
- If `playerHP` is `0` or below, initiates game-over logic (placeholder in the current script).

`AdjustLife(int Amount)`
Adjusts the player's health and updates the health UI:
1. Destroys all current health icons.
2. Updates the `playerHP` value by the specified amount (`Amount`).
3. Recreates health icons based on the updated `playerHP`.

### Adjusting Health In-Game
``` csharp
// Reduce health by 1
playerLifeManager.AdjustLife(-1);

// Increase health by 2
playerLifeManager.AdjustLife(2);
```
