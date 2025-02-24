using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// @author Sarra Demnati
/// 01/04/2025
/// This script is for entering a trigger. With this script, you can add an event.
/// Link to documentation: N/A
/// </summary> 
public class EnterTrigger : MonoBehaviour
{
    //Sarra Demnati
    [SerializeField] private string triggerTag;
    [SerializeField] private UnityEvent enterTrigger;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == triggerTag)
        {
            enterTrigger.Invoke();
        }
    }
}
