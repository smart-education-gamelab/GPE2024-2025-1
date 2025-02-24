using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectionCollisionCheck : MonoBehaviour
{
    public MathManager manager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "MathLineX" && collision.gameObject.tag != "MathLineY") return;
        if (gameObject.tag == collision.gameObject.tag) return;
        Debug.Log("Collision found!");
        manager.intersectPos = new Vector2(transform.position.x, transform.position.y);
        manager.DrawDottedIntersection(new Vector2(transform.position.x, transform.position.y));
    }
}
