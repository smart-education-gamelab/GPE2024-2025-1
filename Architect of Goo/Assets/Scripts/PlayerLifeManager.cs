using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// @author Bram van den Dongen
/// 5/12/2024
/// This is the script that manages the players life and allows other scripts to adjust the life of the player via the function AdjustLife()
/// Link to documentation: N.A.
/// </summary> 

public class PlayerLifeManager : MonoBehaviour
{
    [SerializeField]
    private int playerHP;
    [SerializeField]
    private GameObject healthPrefab;
    [SerializeField]
    private Canvas canvas;

    private List<GameObject> healthObjects;


    void Start()
    {
        healthObjects = new List<GameObject>();
        //Start with a loop to make instantiate a prefab for every HP the player has
        for(int i = 0; i < playerHP; i++)
        {
            var healthIcon = Instantiate(healthPrefab);
            healthIcon.transform.SetParent(canvas.transform);
            healthIcon.transform.position = new Vector3(75 * i + 50, 50, 0);
            healthObjects.Add(healthIcon);
        }
    }

    private void Update()
    {
        // If the player has 0 HP switch the scene to a gameover screen or something like that
        if(playerHP <= 0)
        {
            // GameOver
        }
    }

    // This is the function to adjust the players life
    public void AdjustLife(int Amount)
    {
        // First clear out the existing objects
        foreach(var healthObject in healthObjects)
        {
            Destroy(healthObject);
        }
        // Then adjust the life
        playerHP += Amount;
        // Then make the new icons
        for (int i = 0; i < playerHP; i++)
        {
            var healthIcon = Instantiate(healthPrefab);
            healthIcon.transform.SetParent(canvas.transform);
            healthIcon.transform.position = new Vector3(75 * i + 50, 50, 0);
            healthObjects.Add(healthIcon);
        }
    }
}
