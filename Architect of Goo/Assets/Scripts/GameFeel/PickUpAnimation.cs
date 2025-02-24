using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @author Sarra Demnati
/// 01/04/2025
/// This script manages the particles and sounds of pickup actions (additional extras can be added). 
/// The particles and sounds are either relocated or set to active/inactive states.
/// To add new extras: 
/// If the extra is not supposed to play when the inventory is full, 
/// go to the Inventory script and add the following at the end of the FillInventory function: 
/// the bool statement `allSlotsFull`.
/// Link to documentation: N.A.
/// </summary>

public class PickUpAnimation : MonoBehaviour
{
    [SerializeField] private GameObject partical;
    [SerializeField] private GameObject popSound;
    [SerializeField] private ParticleSystem particalAnimation;


    private void Update()
    {
        //make sure sound and animation stop 
        if (particalAnimation.isStopped)
        {
            partical.SetActive(false);
            popSound.SetActive(false);
        }
    }
    public void StartingAnimation() 
    {
        //change location of the object
        partical.transform.position = this.transform.position;
        partical.SetActive(true);
    }
}
