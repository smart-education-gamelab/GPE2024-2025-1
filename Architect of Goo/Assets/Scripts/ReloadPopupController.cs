using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// @author Ruben Verheul
/// 12/12/2024
/// This script handles the win condition of the game. AKA reloading the level on button press.
/// Link to documentation: N.A.
/// </summary>
public class ReloadPopupController : MonoBehaviour
{
    public GameObject PopupPanel;
    private bool _isPlayerInArea = false;

    private void Start()
    {
        if (PopupPanel != null)
        {
            PopupPanel.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            _isPlayerInArea = true;
            if (PopupPanel != null)
            {
                PopupPanel.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            _isPlayerInArea = false;
            if (PopupPanel != null)
            {
                PopupPanel.SetActive(false);
            }
        }
    }

    public void ReloadLevel()
    {
        if (_isPlayerInArea)
        {
            if (SceneManagerController.Instance != null)
            {
                SceneManagerController.Instance.ReloadCurrentScene(true, true);
            }
            else 
            {
                Debug.LogWarning("SceneManagerController.Instance is not available. Reloading using SceneManager directly.");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
