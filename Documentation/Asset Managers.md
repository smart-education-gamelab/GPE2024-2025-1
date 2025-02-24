The assets are currently have two different managers. The **Conveyor Belt Manager** and the **Lamp Manager**. These are two different asset managers that have quite a lot of overlap, but had some differences. It would be wise to create a **Platform Manager** based on these two managers, but due to having little time correctly translate the **Conveyor Belt Manager** to support multiple platform types with different assets a separate **Lamp Manager** has been created.

These managers are easy to use. You can drop them into the scene and they already have all the components assigned to them. You can determine their **Length**, whether they use a **Foundation** and the **Height** of said foundation. In the **Lamp Manager**'s case, it also has booleans for whether or not they should have a **grapplepoint**, a **cannon platform**, or it should flip horizontally. The functionalities of the grapplepoint and cannon platform have to manually be implemented, these are purely aesthetics.

The platforms are created within the editor, allowing level designers to easily change components of these platforms. The platforms are split into multiple parts. The **LeftPiece**, the **MiddlePieces**, and the **RightPiece**. Of course, if the platform has a foundation, there are three corresponding parts similar to the standard three parts.

The generation of the platform goes from **left** to **right**. Once the **left piece** has been generated, the **middlepieces** will be generated. Based on the length of the platform, the middlepiece will be places and correctly spaced apart. These parts also check whether it should be a **foundation** piece or a **normal** piece. After which it will generate the **right piece**. 

During this process, the script calculates the actual **width** of the platform. It uses this width variable to generate a custom **collider** that is sized based on the width of the platform.

The **piece prefabs** are located in **Assets/Prefabs/Platforms**.

My suggestion in order to create a more dynamic platform creator, is to make changes to the **Lamp Manager** since this script is less restricting than the **Conveyor Belt Manager**.