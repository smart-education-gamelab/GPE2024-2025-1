using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// @author Benjamin van Kessel
/// 10/12/2024
/// This script is a collider checking script that communciates whether or not there is a trigger to the connected SolutionZone.
/// Link to documentation: N.A.
/// </summary> 

public class GrapplePoint : MonoBehaviour
{
    public SolutionZone solutionZone;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Intersection") return;
        solutionZone.intersectOnGrapple = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        solutionZone.intersectOnGrapple = false;
    }
}
