using System.Collections.Generic;
using UnityEngine;
using System;
using org.matheval;
using Unity.VisualScripting;
using System.Drawing;
using UnityEngine.UIElements;
using System.Collections;
/// <summary>
/// @author Benjamin van Kessel
/// 8/12/2024
/// This script manages the processing of the formulas the player uses and turns them into lines. The script also calculates interjection points for grapple point interaction.
/// Link to documentation: N.A.
/// </summary> 

public class MathManager : MonoBehaviour
{
    // Variables for regular references
    public GameObject Axis;
    public bool activateGraph = false;
    public List<GameObject> lineObjects = new List<GameObject>();
    public GameObject indicatorObjX;
    public GameObject indicatorObjY;
    public Material indicatorRed;
    public Material indicatorBlue;
    public GameObject graphLineRendererPrefab;
    public GameObject intersectionPoint;
    public Vector2 sZonePos = new Vector2 (0, 0);
    public GameObject intersectObj;
    public bool lineNotBlocked = true;
    
    // Variables for formulas en calculations
    [Header("Formulas")]
    public string X;
    public string Y;
    public float formulaIncrements = 0.5f;
    private List<Vector2> xCoordinates = new List<Vector2>();
    private List<Vector2> yCoordinates = new List<Vector2>();
    private float startingPosX = 0f;
    private float endPosX = 20f;
    private float startingPosY = 0f;
    private float endPosY = 20f;
    private float positionOffset = 40f;

    // Variables for storing data used in line generation
    [Header("Lines")]
    private Vector2 x1 = new Vector2();
    private Vector2 x2 = new Vector2();
    private Vector2 y1 = new Vector2();
    private Vector2 y2 = new Vector2();
    public bool canDrawLines = true;
    public List<GameObject> dotsX = new List<GameObject>();
    public List<GameObject> dotsY = new List<GameObject>();
    private bool canDrawIntersect = false;
    public bool intersectFound = false;
    public Vector2 intersectPos = new Vector2();

    // Recieves the given data from the UI X/Y input elements and runs the calculations
    public void SendData(string recievedX, string recievedY, Vector3 position)
    {
        X = recievedX;
        Y = recievedY;
        //Debug.Log($"Recieved data (X:{recievedX} & Y:{recievedY})");
        sZonePos = position;
        startingPosX = (int)position.x - positionOffset;
        endPosX = (int)position.x + positionOffset;
        startingPosY = (int)position.y - positionOffset;
        endPosY = (int)position.y + positionOffset;

        RunCalculations();
    }

    // Core of the script; runs all the relevant functions
    public void RunCalculations()
    {
        ResetLines();
        CalculateLineX(X);
        CalculateLineY(Y);
        if (xCoordinates.Count == 0 || yCoordinates.Count == 0) return;
        //DrawLines(xCoordinates, yCoordinates);
        //CheckForIntersection(X,Y);
        DrawDottedLines(xCoordinates, yCoordinates, indicatorObjX, indicatorObjY);
        canDrawIntersect = true;
    }

    // Draws the intersection object if there is a dotted line collision on given position
    public void DrawDottedIntersection(Vector2 pos)
    {
        if (!lineNotBlocked) return;
        GameObject intersectPointObj = Instantiate(intersectionPoint, pos, new Quaternion(0, 0, 0, 0));
        Debug.Log($"intersect pos: {pos}");
        intersectPointObj.transform.parent = transform;
        lineObjects.Add(intersectPointObj);
        intersectObj = intersectPointObj;
        canDrawIntersect = false;
    }

    // Draws the dotted line variety of the graphical formulas; works with collision for intersection instead of calculating it
    // Can preform intersections for paraboles
    private void DrawDottedLines(List<Vector2> xCoordinates, List<Vector2> yCoordinates, GameObject indicatorX, GameObject indicatorY)
    {
        int xCount = 0;
        foreach (Vector2 point in xCoordinates)
        {
            GameObject newObj = Instantiate(indicatorX, new Vector2(point.x, point.y), new Quaternion(0, 0, 0, 0));
            newObj.transform.parent = transform;
            newObj.name = "indicatorX" + xCount.ToString();
            xCount++;

            IntersectionCollisionCheck script = newObj.GetComponent<IntersectionCollisionCheck>();
            script.manager = gameObject.GetComponent<MathManager>();

            lineObjects.Add(newObj);
            dotsX.Add(newObj);
        }

        int yCount = 0;
        foreach (Vector2 point in yCoordinates)
        {
            GameObject newObj = Instantiate(indicatorY, new Vector2(point.x, point.y), new Quaternion(0, 0, 0, 0));
            newObj.transform.parent = transform;
            newObj.name = "indicatorY" + yCount.ToString();
            yCount++;

            IntersectionCollisionCheck script = newObj.GetComponent<IntersectionCollisionCheck>();
            script.manager = gameObject.GetComponent<MathManager>();

            lineObjects.Add(newObj);
            dotsY.Add(newObj);
        }
    }

    // Removes parts of the line blocked by lineblockers
    public void RemoveBlockedLines(GameObject blockedLine)
    {
        bool containsLine = false;
        if (dotsX.Contains(blockedLine))
        {
            containsLine = true;
            for (int i = dotsX.Count - 1; i > 0; i--)
            {
                if (dotsX[i] == blockedLine) break;
                GameObject objToBeDestroyed = dotsX[i];
                Destroy(objToBeDestroyed);
                dotsX.RemoveAt(i);
                lineObjects.Remove(objToBeDestroyed);
            }
        }
        else Debug.Log("Not X");

        if (dotsY.Contains(blockedLine))
        {
            containsLine = true;
            for (int i = dotsY.Count - 1; i > 0; i--)
            {
                if (dotsY[i] == blockedLine) break;
                GameObject objToBeDestroyed = dotsY[i];
                Destroy(objToBeDestroyed);
                dotsY.RemoveAt(i);
                lineObjects.Remove(objToBeDestroyed);
            }
        }
        else Debug.Log("Not Y");

        if (containsLine)
        {
            intersectFound = false;
            intersectPos = Vector3.zero;
            //Destroy(intersectObj);
            DestroyImmediate(intersectObj,true);
            lineNotBlocked = false;
            canDrawIntersect = false;
            Debug.Log("Destroyed intersect");
        }
    }

    // Resets the drawn lines so that the lists can be reused
    public void ResetLines()
    {
        xCoordinates.Clear();
        yCoordinates.Clear();
        lineNotBlocked = true;
        
        foreach(GameObject obj in lineObjects)
        {
            Destroy(obj);
        }
        lineObjects.Clear();
    }

    // Toggles the active state of the X/Y axis shown during Solution Zone interactions for guidance during math
    public void ToggleGraph()
    {
        Axis.SetActive(!Axis.active);
    }

    // Calculates the positions of the individual coordinates for the Y line
    private void CalculateLineY(string formula)
    {
        try
        {
            List<Vector2> lineCoordinates = new List<Vector2>();

            formula.Replace(" ", string.Empty);

            for (float x = startingPosX; x < endPosX; x += formulaIncrements)
            {
                Expression expression = new Expression(formula);

                expression.Bind("x", x);
                expression.Bind("X", x);
                System.Object value = expression.Eval();

                float yCoordinate = float.Parse(value.ToString());
                Vector2 newPos = new Vector2(x, yCoordinate);

                lineCoordinates.Add(newPos);
            }

            foreach (var line in lineCoordinates)
            {
                yCoordinates.Add(line);
            }
        }
        catch(Exception ex)
        {
            Debug.Log($"Calculation Y failed! -> {ex}");
        }
    }

    // Calculates the positions of the individual coordinates for the X line
    private void CalculateLineX(string formula)
    {
        try
        {
            List<Vector2> lineCoordinates = new List<Vector2>();

            formula.Replace(" ", string.Empty);

            for (float y = startingPosY; y < endPosY; y += formulaIncrements)
            {
                Expression expression = new Expression(formula);

                expression.Bind("y", y);
                expression.Bind("Y", y);
                System.Object value = expression.Eval();

                float xCoordinate = float.Parse(value.ToString());
                Vector2 newPos = new Vector2(xCoordinate, y);

                lineCoordinates.Add(newPos);
            }

            foreach (var line in lineCoordinates)
            {
                xCoordinates.Add(line);
            }
        }
        catch (Exception ex)
        {
            Debug.Log($"Calculation X failed! -> {ex}");
        }
    }

    // Calculates the intersection from the two given formulas for X and Y
    // Deprecated; cannot calculate intersection for paraboles
    private void CheckForIntersection(string formulaX, string formulaY)
    {
        try
        {
            PointF p1 = new PointF(x1.x, x1.y);
            PointF p2 = new PointF(x2.x, x2.y);
            PointF p3 = new PointF(y1.x, y1.y);
            PointF p4 = new PointF(y2.x, y2.y);
            Vector2 intersectVector;
            bool foundIntersect;
            FindIntersection(p1, p2, p3, p4, out intersectVector, out foundIntersect);
            if (!foundIntersect || Vector2.Distance(sZonePos, intersectVector) > 12) return;

            GameObject intersectPointObj = Instantiate(intersectionPoint, intersectVector, new Quaternion(0, 0, 0, 0));
            intersectPointObj.transform.parent = transform;
            lineObjects.Add(intersectPointObj);
        }
        catch(Exception ex)
        {
            Debug.Log($"Intersection failed! -> ({ex})");
        }
    }

    // Find the point of intersection between the lines p1 --> p2 and p3 --> p4.
    // Deprecated; part of CheckForIntersection()
    private void FindIntersection(
        PointF p1, PointF p2, PointF p3, PointF p4,
        out Vector2 intersectVector, out bool foundIntersect)
    {
        // Get the segments' parameters.
        float dx12 = p2.X - p1.X;
        float dy12 = p2.Y - p1.Y;
        float dx34 = p4.X - p3.X;
        float dy34 = p4.Y - p3.Y;

        // Solve for t1 and t2
        float denominator = (dy12 * dx34 - dx12 * dy34);

        float t1 =
            ((p1.X - p3.X) * dy34 + (p3.Y - p1.Y) * dx34)
                / denominator;
        if (float.IsInfinity(t1))
        {
            // The lines are parallel (or close enough to it).
            Debug.Log("Lines are parallel");
            intersectVector = new Vector2();
            foundIntersect = false;
            return;
        }

        float t2 =
            ((p3.X - p1.X) * dy12 + (p1.Y - p3.Y) * dx12)
                / -denominator;

        // Find the point of intersection.
        foundIntersect = true;
        intersectVector = new Vector2(p1.X + dx12 * t1, p1.Y + dy12 * t1);
    }

    // Draws grapgical formula lines based on given X and Y
    // Deprecated; uses math to find intersection but cannot do paraboles
    private void DrawLines(List<Vector2> xCoordinates, List<Vector2> yCoordinates)
    {
        try
        {
            Vector2 previousX = new Vector2(0, 0);
            Vector2 previousY = new Vector2(0, 0);

            GameObject xLine = Instantiate(graphLineRendererPrefab);
            GameObject yLine = Instantiate(graphLineRendererPrefab);
            lineObjects.Add(xLine);
            lineObjects.Add(yLine);
            LineRenderer rendererX = xLine.GetComponent<LineRenderer>();
            LineRenderer rendererY = yLine.GetComponent<LineRenderer>();
            EdgeCollider2D colliderX = xLine.GetComponent<EdgeCollider2D>();
            EdgeCollider2D colliderY = yLine.GetComponent<EdgeCollider2D>();
            xLine.transform.parent = transform;
            yLine.transform.parent = transform;
            rendererX.material = indicatorRed;
            rendererY.material = indicatorBlue;

            xLine.name = "GraphLineX";
            yLine.name = "GraphLineY";

            Vector3[] xArray = new Vector3[xCoordinates.Count];
            Vector3[] yArray = new Vector3[yCoordinates.Count];

            for (int i = 0; i < xCoordinates.Count; i++)
            {
                xArray[i] = xCoordinates[i];
            }

            for (int i = 0; i < yCoordinates.Count; i++)
            {
                yArray[i] = yCoordinates[i];
            }

            if (xArray.Length != 0 && yArray.Length != 0)
            {
                rendererX.positionCount = xArray.Length;
                rendererY.positionCount = yArray.Length;

                rendererX.SetPositions(xArray);
                rendererY.SetPositions(yArray);
                colliderX.SetPoints(xCoordinates);
                colliderY.SetPoints(yCoordinates);

                x1 = rendererX.GetPosition(0);
                x2 = rendererX.GetPosition(rendererX.positionCount - 1);
                y1 = rendererY.GetPosition(0);
                y2 = rendererY.GetPosition(rendererY.positionCount - 1);
            }
        }
        catch (Exception e)
        {
            Debug.Log($"Drawing failed! -> {e}");
        }
    }
}
