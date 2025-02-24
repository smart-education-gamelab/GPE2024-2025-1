using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineBreaker : MonoBehaviour
{
    public MathManager manager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "MathLineX" && collision.gameObject.tag != "MathLineY") return;
        Debug.Log($"Check - {collision.gameObject} - {collision.gameObject.name}");
        manager.RemoveBlockedLines(collision.gameObject);
    }
}
