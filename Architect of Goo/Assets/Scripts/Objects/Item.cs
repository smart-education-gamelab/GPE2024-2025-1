using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// @author Sarra Demnati
/// 01/04/2025
/// This script creates items for the inventory.
/// Link to documentation: N/A
/// </summary>

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
}
