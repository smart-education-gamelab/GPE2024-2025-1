using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public enum groundType
    {
        ConveyorBelt,
        Slime
    }
    public groundType currentGround = groundType.ConveyorBelt;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Ground")
        {
            currentGround = groundType.ConveyorBelt;
        }
        if(collision.tag == "SlimeBridge")
        {
            currentGround = groundType.Slime;
        }
    }
}
