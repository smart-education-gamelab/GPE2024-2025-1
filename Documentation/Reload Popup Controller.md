The **Reload Popup Controller** highlights a **panel** where the player can press a button to reload the level or progress to a new scene. This component is still called **Reload** since that was its original use, but is now used in other ways.

The **Reload Popup Controller** is assigned on the **LevelEnd** prefab. When dragging it into the scene, you have to assign the **PopupPanel** located in the player prefab at **Player/Main Camera/UI/PopupPanel**. This popup panel holds a **ReloadButton** where you can add a new event to listen to from the **SceneManagerController**.

The controller itself checks whether the **player** is within the **LevelEnd collider**. Based on whether or not the player is inside this zone, the **popup panel** will appear before the player.