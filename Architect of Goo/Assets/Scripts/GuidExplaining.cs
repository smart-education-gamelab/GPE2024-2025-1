using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// @author Sarra Demnati
/// 01/04/2025
/// This script is for truning for the values of the Guide line and text box to explain where the numbers come from. 
/// Link to documentation: N/A
/// </summary> 

public class GuidExplaining : MonoBehaviour
{
    [SerializeField] private GameObject guidBox, guidLine;
    [SerializeField] private float timeToReadGuid;
    [SerializeField] private TextMeshProUGUI variableText;
    [SerializeField]private SolutionZone solutionZone;
    [SerializeField] private Vector2 offset;
    public int xAStart, yAStart, xBStart, yBStart;
    public float rc;
    private float negative = 1;

    public UnityEvent uiIsOn;

    public void VisableGuideBox()
    {

        guidBox.SetActive(true);
        guidLine.SetActive(true);
        print(solutionZone.xA);
        //get values for and safe
         xAStart = Mathf.RoundToInt(solutionZone.xA);
         yAStart = Mathf.RoundToInt(solutionZone.yA);

         xBStart = Mathf.RoundToInt(solutionZone.xB);
         yBStart = Mathf.RoundToInt(solutionZone.yB);

        rc =(float) (yBStart - yAStart) / (xBStart - xAStart);

        variableText.text = $"Xb: {xBStart} - Xa: {xAStart} = {xBStart - xAStart} \n Yb: {yBStart} - Ya: {yAStart} = {yBStart - yAStart} \n (Yb-Ya)/(Xb-Xa) = {rc} ";

        uiIsOn.Invoke();


        if (rc < 0)
        {
             negative = -1;
        }
        //Horizontal Line
        LineRendering(new Vector2 (xAStart, yAStart -  rc - 1), new Vector2(xAStart + 1 * negative + 0.3f ,yAStart - rc -1));
        TextCreating("Horizontal information","X" , new Vector2((xAStart + 0.5f), yAStart - 2));
        //Vertical Line
        LineRendering(new Vector2(xAStart + 1 + offset.x, yAStart - 0.7f) , new Vector2(xAStart + 1 + offset.x , yAStart - rc -1 + offset.y));
        TextCreating("Vertical information (RC)", rc.ToString() , new Vector2((xAStart + 1.5f), rc - 1 - offset.y-(rc/2)));
        StartCoroutine(ReadGuid());
    }

    public void TextCreating(string nameGameObject, string text, Vector2 position)
    {
        GameObject textGameobject = new GameObject(nameGameObject);
        TextMesh textMesh = textGameobject.AddComponent<TextMesh>();

        textMesh.text = text;
        textMesh.color = Color.black;
        textMesh.fontSize = 7;
        textMesh.fontStyle = FontStyle.Bold;

        textGameobject.transform.position = new Vector3(position.x,position.y,0);
    }
    public void LineRendering(Vector2 positionStart, Vector2 positionEnd)
    {
        GameObject lineObject = new GameObject("NewLine");
        LineRenderer lineRender =  lineObject.AddComponent<LineRenderer>();

        lineRender.material = new Material(Shader.Find("Sprites/Default"));
        lineRender.startColor = Color.black;
        lineRender.endColor = Color.black;
        lineRender.widthMultiplier = 0.3f;

        lineRender.positionCount = 2;
        lineRender.SetPosition(0, positionStart);
        lineRender.SetPosition(1, positionEnd);
    }


    public IEnumerator ReadGuid()
    {
        yield return new WaitForSeconds(timeToReadGuid);
        guidBox.SetActive(false);
        guidLine.SetActive(false);
    }
}
