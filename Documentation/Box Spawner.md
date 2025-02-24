The **Box Spawner** is an empty gameobject that spawns in a box at the location of the spawner. The Box Spawner prefab holds a box prefab, which is used for referencing and spawning the box. 

The script checks whether the box has the boolean **Respawn** set to true. When it is set to true, it will destroy the currently spawn box and replace it with a new box.

The functionality of detecting when it should respawn is not implemented yet.

The Box Spawner is a feature that could be used to create more gameplay related puzzles (such as moving the boxes on pressure plates to unlock something), but was put on the backburner during this project.