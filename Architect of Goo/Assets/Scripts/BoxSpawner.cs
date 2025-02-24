using UnityEngine;

/// <summary>
/// @author Ruben Verheul
/// 09/12/2024
/// This script handles the creation of boxes.
/// Link to documentation: N.A.
/// </summary>
public class BoxSpawner : MonoBehaviour
{
    public GameObject BoxPrefab;
    public bool Respawn = false;
    private GameObject _currentBox;

    private void Start()
    {
        SpawnBox();
    }

    private void Update()
    {
        if (Respawn)
        {
            Respawn = false;
            SpawnBox();
        }
    }
    
    private void SpawnBox()
    {
        if (_currentBox != null)
        {
            Destroy(_currentBox);
        }
        _currentBox = Instantiate(BoxPrefab, transform.position, Quaternion.identity);
    }

}
