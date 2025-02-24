using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GuideDirection : MonoBehaviour
{
    /// <summary>
    /// @author Sarra Demnati
    /// 01/04/2025
    /// This script is for truning the arrow so it is clear whiche direction the RC is going. It is for creating a visable look of how the formula works and why.
    /// Link to documentation: N/A
    /// </summary> 
    [SerializeField] private GuidExplaining guidScipt;
    [SerializeField] public Vector2 offSetObject;
    [SerializeField] private GameObject canon;

    public void Turing()
    {
        transform.position = new Vector3(canon.transform.position.x - offSetObject.x, canon.transform.position.y - offSetObject.y,transform.position.z);
        transform.Rotate(transform.localRotation.x, transform.localRotation.y,- Mathf.Atan2(guidScipt.rc, 1) * Mathf.Rad2Deg);
        print(guidScipt.rc);
        float size = math.sqrt((float)guidScipt.rc * (float) guidScipt.rc);
        transform.localScale = new Vector3(size * 0.3f,transform.localScale.y,transform.localScale.z);
    }
}
