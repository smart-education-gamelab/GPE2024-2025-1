using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// @author Benjamin van Kessel
/// 10/12/2024
/// This script is used to check for player collision and to communicate with the UI and the MathManager.
/// Link to documentation: N.A.
/// </summary> 

public class SolutionZone : MonoBehaviour
{
    public FormulaUIManager formulaUIManager;
    public MathManager mathManager;
    public string X;
    public string Y;
    public bool intersectOnGrapple = false;
    public LineRenderer lineRenderer;
    private bool hasEntered = false;
    private GameObject player = null;    
    [SerializeField] private RewindSolution rewindScript;
    public ParticleSystem slimeCannon;
    public bool isSolved = false;

    //Guid
    public float xA, xB, yA, yB;
    public GuidExplaining guideScript;


    public void UpdateGraph(string xString, string yString)
    {
        if (!hasEntered) return;
        X = xString;
        Y = yString;
        mathManager.SendData(xString, yString, transform.position);
        formulaUIManager.currentSolutionZone = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player" || isSolved) return;
        //Debug.Log("Player entered");
        //formulaUIManager.ToggleFormulaUI(true);
        mathManager.ToggleGraph();
        hasEntered = true;
        if (player == null) player = collision.gameObject;

        //Notify the camera to zoom out
        CameraPanController.Instance.EnterSolutionZone(transform.GetChild(0));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player" || isSolved) return;
        formulaUIManager.ToggleFormulaUI(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player" || isSolved) return;
        formulaUIManager.ToggleFormulaUI(false);
        mathManager.ToggleGraph();
        hasEntered = false;
        collision.gameObject.transform.GetChild(1).gameObject.GetComponent<Inventory>().DeactivateItems(); // Get the inventory script from player child, start deactivation.

        //Notify the camera to reset
        CameraPanController.Instance.ExitSolutionZone();
    }

    public void TrySolution()
    {
        if (!intersectOnGrapple) return;
        Debug.Log("Success!");
        Inventory invScript = player.transform.GetChild(1).gameObject.GetComponent<Inventory>();
        invScript.RemoveActivatedItems();
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        formulaUIManager.ToggleFormulaUI(false);
        mathManager.ResetLines();
        rewindScript.ShowRewindIndicator(invScript);

        GameObject grapplePoint = gameObject.transform.GetChild(0).gameObject;
        GameObject cannonPoint = lineRenderer.gameObject;
        Vector2 grapplePos = grapplePoint.transform.position;
        Vector2 cannonPos = cannonPoint.transform.position;
        
        lineRenderer.SetPosition(0, cannonPos);
        
        slimeCannon.Play();
        //Start the coroutine to shoot the bridge
        StartCoroutine(ShootBridge(cannonPos,grapplePos,25,0.5f));

        xA = cannonPos.x;
        xB = grapplePos.x;

        yA = cannonPos.y;
        yB = grapplePos.y;

        if (!guideScript.IsUnityNull())
        {
            guideScript.VisableGuideBox();
        }

        EdgeCollider2D collider = cannonPoint.gameObject.GetComponent<EdgeCollider2D>();
        collider.enabled = true;
        Vector2 p1 = cannonPos - cannonPos;
        Vector2 p2 = grapplePos - cannonPos;
        Vector2[] colliderPos = new Vector2[2] { p1, p2 };
        collider.points = colliderPos;

        
    }

    IEnumerator ShootBridge(Vector2 startPosition, Vector2 endPosition, float steps, float duration)
    {
        // Wait for an amount of seconds of the duration devided by the amount of steps
        yield return new WaitForSeconds(duration / steps);

        var currentPosition = startPosition;

        // calculate the distances for x and y axis
        var xDistance = endPosition.x - startPosition.x;
        var yDistance = endPosition.y - startPosition.y;

        // calculate the step needed to cross the correct distance
        var xStep = xDistance / steps;
        var yStep = yDistance / steps;

        // adjust the position and render the new line
        currentPosition = new Vector2(currentPosition.x + xStep, currentPosition.y + yStep);
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, currentPosition);

        // if the last position has been reached end the coroutine
        if (currentPosition == endPosition)
        {
            yield break;
        }
        else
        {
            // if the last spot hasn't been reached repeat the process with the duration and steps reduced
            yield return StartCoroutine(ShootBridge(currentPosition, endPosition, steps - 1, duration - duration / steps));
        }
    }
}
