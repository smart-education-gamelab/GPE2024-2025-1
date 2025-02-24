using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// @author Ruben Verheul
/// 11/12/2024
/// This script reloads the level once the player has enter the field.
/// Link to documentation: N.A.
/// </summary>
public class ReloadLevel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            SceneManagerController.Instance.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
