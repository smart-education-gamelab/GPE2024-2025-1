The **Camera Pan Controller** is used for expanding the view of the camera when using the solution zones.

The controller grabs the **grapplepoint** that is linked to the **solution zone** the player is currently in. It calculates the **vertical** and **horizontal distances** between the player and the grapplepoint to determine a suitable camera size based on the distances.

Based on these calculations, the camera size will be adjusted by using the **lerp** function, making sure the camera changes feel smooth.

This script is assigned to the **Player** prefab, on the **Main Camera** component. You will have to assign the **Player Transform** into the **Player** variable in the inspector.