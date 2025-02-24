The **Scene Manager Controller** handles the general scene management, such as (re)loading scenes or using a transition fade effect.

If you want to switch scenes, call the **LoadScene** function. This will allow you to fill in a **sceneName**, whether or not you want it to be a **asynchronic** event, and to use the **fade effect**.

When using the **fade effect**, it will start the **fade coroutine**. In this coroutine a **canvas**, that is assigned to the scene manager, will transition from their **startAlpha** to the given **targetAlpha**. If the **targetAlpha** is **'1'**, it will turn the canvas to **black**. If it is **'0'**, it will slowly turn to **transparent**.

In order to use the **Scene Manager Controller**, you can drop the **SceneManagerController** prefab into the scene. It should already hold the correct.

It also holds a **QuitGame** function, which allows the player to close the application. This is currently being checked in the **Update** cycle.