using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// @author Bram van den Dongen
/// 19/12/2024
/// This is the script that links the players respawn system to this location
/// Link to documentation: N.A.
/// </summary> 
public class Checkpoint : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool pointActive = false;

    private void Start()
    {
        //get the sprite renderer for visual feedback
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //check if the colliding object is the player
        if (collision.gameObject.GetComponent<PlayerRespawn>() != null)
        {
            pointActive = true;
            var player = collision.gameObject.GetComponent<PlayerRespawn>();
            // see if the player was already using a checkpoint that isnt this point
            if (player.usingCheckpoint && player.currentCheckpoint != this)
            {
                player.currentCheckpoint.pointActive = false;
            }
            else
            {
                player.usingCheckpoint = true;
            }
            // set this as the current checkpoint
            player.currentCheckpoint = this;
            player.checkpointPosition = transform.position;
        }
    }

    private void Update()
    {
        //give visual feedback if this is the object is active
        if (pointActive)
        {
            spriteRenderer.color = Color.yellow;
        } else
        {
            spriteRenderer.color = Color.red;
        }
    }
}
