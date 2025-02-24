using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindSolution : MonoBehaviour
{
    [SerializeField] private SolutionZone sZone;
    [SerializeField] private GameObject rewindIndicator;
    [HideInInspector] public BoxCollider2D boxCollider;
    private Inventory invScript;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;
        rewindIndicator.SetActive(false);
    }

    public void ShowRewindIndicator(Inventory inv)
    {
        sZone.isSolved = true;
        rewindIndicator.SetActive(true);
        boxCollider.enabled = true;
        invScript = inv;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && Input.GetKey(KeyCode.W))
        {
            // Undoing solution
            sZone.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            sZone.lineRenderer.positionCount = 0;
            sZone.lineRenderer.positionCount = 2;
            sZone.formulaUIManager.xText.text = "";
            sZone.formulaUIManager.yText.text = "";

            EdgeCollider2D collider = sZone.slimeCannon.gameObject.GetComponent<EdgeCollider2D>(); 
            collider.enabled = false;

            int index = -1;
            UsedVariables reclaimedVariables = null;
            // Giving back used variables
            foreach(UsedVariables uVar in invScript.usedVariablesList)
            {
                index++;
                if (!uVar.connectedSolutionZone == sZone) continue;
                Debug.Log($"Current solution zone index: {index}");
                reclaimedVariables = uVar;
                foreach(GameObject item in uVar.storedVariables)
                {
                    item.SetActive(true);
                    invScript.ReturnToSlot(item);
                }
            }

            if (reclaimedVariables != null) invScript.usedVariablesList.Remove(reclaimedVariables);

            //Rehiding rewind
            boxCollider.enabled = false;
            rewindIndicator.SetActive(false);
            sZone.isSolved=false;
        }
    }
}
