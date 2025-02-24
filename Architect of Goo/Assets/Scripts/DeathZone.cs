using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// @author Bram van den Dongen
/// 10/12/2024
/// This the script that causes an area to respawn the player at either their most recent point or another specified point
/// Link to documentation: N.A.
/// </summary> 

public class DeathZone : MonoBehaviour
{
    // when an object collides, check if it has the respawn component, if so respawn them.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerRespawn>() != null)
        {
            var player = collision.gameObject.GetComponent<PlayerRespawn>();
            player.Respawn();
        }
    }
}
