using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// @author Sarra Demnati
/// 01/04/2025
/// This script manages the inventory. If a new slot is added to the UI, the inventory automatically grows. It checks the slot
/// and does not work with an array. Items are removed using Unity's button function.
/// Link to documentation: Assets>Documentation>InventoryDocumentation
/// </summary>
public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject uiInventory;
    [SerializeField] private GameObject storedVariableObject;
    [SerializeField] private GameObject particalPickUp;
    [SerializeField] private GameObject popSound;
    [SerializeField] private FormulaUIManager formulaUIManager;
    private List<GameObject> activatedSlots = new List<GameObject>();
    public List<UsedVariables> usedVariablesList = new List<UsedVariables>();

    public void FillSlot(GameObject inventoryItem)
    {
        bool allSlotsFull = true;
        for (int i = 0; i < uiInventory.transform.childCount; i++)
        {    
            Transform slot = uiInventory.transform.GetChild(i); // Get each slot
            // Check if the slot has any children
            if (slot.childCount < 1)
            {
                //Gets the information of the object
                ItemController itemControler = inventoryItem.GetComponent<ItemController>();
                GameObject itemObject = new GameObject(itemControler.item.name); //make a new gameobject

                Image imageItemObject = itemObject.AddComponent<Image>(); //create a place for the image
                //Put the object in place
                itemObject.transform.SetParent(slot.transform);
                imageItemObject.sprite = itemControler.item.icon;
                itemObject.transform.localScale = new Vector2(0.15f, 0.15f);
                itemObject.transform.localPosition = Vector3.zero;

                //remove item that gets picked up
                Destroy(inventoryItem);
                allSlotsFull = false;
                break;
            }
        }
        if (allSlotsFull)
        {
            //when inventory is full dont play animation and sound
            particalPickUp.SetActive(false);
            popSound.SetActive(false);
        }
    }

    public void ReturnToSlot(GameObject item)
    {
        bool allSlotsFull = true;
        for (int i = 0; i < uiInventory.transform.childCount; i++)
        {
            Transform slot = uiInventory.transform.GetChild(i); // Get each slot
            // Check if the slot has any children
            if (slot.childCount < 1)
            {
                //Put the object in place
                item.transform.SetParent(slot.transform);
                item.transform.localScale = new Vector2(0.15f, 0.15f);
                item.transform.localPosition = Vector3.zero;

                allSlotsFull = false;
                break;
            }
        }
        if (allSlotsFull)
        {
            //when inventory is full dont play animation and sound
            particalPickUp.SetActive(false);
            popSound.SetActive(false);
        }
    }

    public void DeactivateItems()
    {
        for (int i = 0; i < uiInventory.transform.childCount; i++)
        {
            GameObject slot = uiInventory.transform.GetChild(i).gameObject; // Get each slot
            if (!activatedSlots.Contains(slot)) continue;
            ToggleSlot(slot, true);
        }
    }

    public void ActivateItem(GameObject slot)
    {
        // Activates item for use in Solution Zone
        if (slot.transform.childCount >= 1 && !activatedSlots.Contains(slot))
        {
            Debug.Log("Activated item for SZ");
            switch (slot.transform.GetChild(0).name)
            {
                case "X":
                    formulaUIManager.yText.text += "X";
                    ToggleSlot(slot, false);
                    break;
                case "Y":
                    formulaUIManager.xText.text += "Y";
                    ToggleSlot(slot, false);
                    break;
                default:
                    Debug.Log("Item could not be defined!");
                    break;
            }
        }
       
    }


    public void RemoveItem(GameObject slot) 
    {
        if (slot.transform.childCount >= 1)
        {
            //delete the object from inventory
            Transform removeObjectTransformUI = slot.transform.GetChild(0);
            GameObject removeObjectUI = removeObjectTransformUI.gameObject;
            Destroy(removeObjectUI);
        }
    }

    public void RemoveActivatedItems()
    {
        // Creating list to store used variables
        List<GameObject> storeVariables = new List<GameObject>();

        for (int i = 0; i < uiInventory.transform.childCount; i++)
        {
            GameObject slot = uiInventory.transform.GetChild(i).gameObject; // Get each slot
            Debug.Log($"Iteration: {i}");
            if (!activatedSlots.Contains(slot)) continue;
            
            // Storing item in usedVariablesList
            GameObject item = slot.transform.GetChild(0).gameObject;
            storeVariables.Add(item);

            // Moving item to storedvariablesobject in hierarchy
            item.SetActive(false);
            item.transform.parent = storedVariableObject.transform;
            //Destroy(slot.transform.GetChild(0).gameObject);
            ToggleSlot(slot, true);
        }

        // Sending all used variables to usedVariablesList
        UsedVariables newUsedVariables = new UsedVariables(formulaUIManager.currentSolutionZone.gameObject, storeVariables);
        usedVariablesList.Add(newUsedVariables);
    }

    private void ToggleSlot(GameObject slot, bool status)
    {
        switch (status)
        {
            case true:
                if (!activatedSlots.Contains(slot)) return;
                slot.transform.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                activatedSlots.Remove(slot);
                break;
            case false:
                if (activatedSlots.Contains(slot)) return;
                slot.transform.GetComponent<Image>().color = new Color32(100, 100, 100, 200);
                activatedSlots.Add(slot);
                break;
        }
    }
}

// Serializable class for saving which solution zone used which variables
// Used to return all used variables, even when other solution zones have been used after.
[Serializable()]
public class UsedVariables
{
    public GameObject connectedSolutionZone;
    public List<GameObject> storedVariables;

    public UsedVariables(GameObject parent, List<GameObject> usedVar) {
        this.connectedSolutionZone = parent;
        this.storedVariables = usedVar;
    }
}
