using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// @author Bram van den Dongen
/// 10/12/2024
/// This the script that manages the respawning of the player
/// Link to documentation: N.A.
/// </summary> 

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField]
    private PlayerLifeManager lifeManager;
    [SerializeField]
    private Playermovement movement;

    private Vector2 lastPoint;

    internal bool usingCheckpoint;
    internal Vector2 checkpointPosition;
    internal Checkpoint currentCheckpoint;

    private void Update()
    {
        // If the player is grounded save the most recent position to be able to reset the position
        if (movement.isGrounded)
        {
            lastPoint = transform.position;
        }
    }
    //Function that respawns the player
    public void Respawn()
    {
        // Adjust the players life by one
        lifeManager.AdjustLife(-1);

        // if a respawn point is used set the position to that point otherwise set the player a bit away from their last point
        if (usingCheckpoint)
        {
            transform.position = checkpointPosition;
        }
        else
        {
            if (transform.position.x < lastPoint.x)
            {
                transform.position = new Vector2(lastPoint.x + 1, lastPoint.y);
            }
            else if (transform.position.x > lastPoint.x)
            {
                transform.position = new Vector2(lastPoint.x - 1, lastPoint.y);
            }
        }
    }
}
