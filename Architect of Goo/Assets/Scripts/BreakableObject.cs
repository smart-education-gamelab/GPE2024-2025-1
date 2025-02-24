using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// @author Bram van den Dongen
/// 17/12/2024
/// This is the script that breaks the wall and starts the particles
/// Link to documentation: N.A.
/// </summary> 

public class BreakableObject : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private Rigidbody2D[] rb;
    [SerializeField]
    [Range(1, 30)] private int launchMin;
    [SerializeField]
    [Range(1, 30)] private int launchMax;

    internal bool isBroken = false;
    private Vector2[] startPositions;
    private Collider2D collider;
    private void Start()
    {
        collider = GetComponent<Collider2D>();

        var sh = particleSystem.shape;
        sh.scale = new Vector3(transform.localScale.x * 3, 1, 1);

        // make an array and store the starting positions of the objects in it
        startPositions = new Vector2[rb.Length];
        for(int i = 0; i < startPositions.Length; i++)
        {
            startPositions[i] = rb[i].transform.localPosition;
        }
    }
    private void Update()
    {
        //Stop the movement of the wall pieces below a certain point
        if (rb[0].position.y <= -40)
        {
            foreach(var rb in rb)
            {
                rb.velocity = Vector3.zero;
                rb.gravityScale = 0f;
            }
        }
    }

    // Function that breaks the wall and starts the particle system
    public void BreakWall()
    {
        //check if the wall isn't broken
        if (isBroken) return;
        isBroken = true;
        particleSystem.Play();
        collider.enabled = false;

        foreach (var rb in rb)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 5f;
            rb.AddForce(new Vector2(Random.Range(launchMin, launchMax), 10), ForceMode2D.Impulse);
        }
    }

    // function that resets the wall only if the wall
    public void ResetWall()
    {
        // check if the wall is broken
        if(!isBroken) return;
        isBroken= false;
        for(int i = 0;i < startPositions.Length;i++)
        {
            rb[i].bodyType= RigidbodyType2D.Kinematic;
            rb[i].gravityScale = 0f;
            rb[i].velocity = Vector2.zero;
            rb[i].transform.localPosition = startPositions[i];
        }
    }
}
