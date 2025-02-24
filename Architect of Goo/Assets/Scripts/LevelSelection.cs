using UnityEngine;
using TMPro;
using UnityEngine.UI;


/// <summary>
/// @author Ruben Verheul
/// 10/01/2024
/// This script generates buttons for the level selection.
/// Link to documentation: N.A.
/// </summary>
public class LevelSelection : MonoBehaviour
{
    public static LevelSelection Instance { get; private set; }
    public GameObject ButtonPrefab;
    public Transform LevelPanel;

    [SerializeField] private int _levels;
    [SerializeField] private string _prefix;

    private void Awake()
    {
        //Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GenerateLevelButtons();
    }

    private void GenerateLevelButtons()
    {
        for (int i = 1; i <= _levels; i++)
        {
            GameObject newButton = Instantiate(ButtonPrefab, LevelPanel);
            newButton.GetComponentInChildren<TMP_Text>().text = "Level " + i;

            int levelIndex = i;
            newButton.GetComponent<Button>().onClick.AddListener(() => LoadLevel(levelIndex));
        }
    }

    private void LoadLevel(int levelIndex)
    {
        SceneManagerController.Instance.LoadScene(_prefix + levelIndex); //This will load a scene that is named "LevelX" where X is the level.
    }
}
