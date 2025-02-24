The **Level Selection Manager** generates buttons for the level selection screen.

It will generate buttons based on the amount of levels assigned in the prefab. These buttons will add an **onClick event listener**. To generate the events correctly, the script combines the predetermined **_prefix** variable with the **levelIndex**. In the current state of the project, the prefix is **"Level "**. With this prefix it will grabs the scenes correctly is they are named **Level X**.

You can use the level selection by grabbing the **LevelSelectorManager** prefab. In this prefab, there are the **Button** prefab, a **Panel** to put the buttons in, a variable to determine the amount of **levels**, and the **prefix**.

It will automatically generated everything correctly to be used immediately, as long as the naming of the scenes are aligned with the prefix. This method has been opted to use to allow level designers to focus on creating the actual levels and only focus on using the correct naming convention. 

If you want to use the **level selector**, you have to create a **canvas** with a **panel component** where the buttons will be generated. Make sure to assign this **panel** to the **Level Selection Manager**.