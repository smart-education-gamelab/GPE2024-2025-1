using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// @author Ruben Verheul
/// 19/12/2024
/// This script handles the scene management. (Re)loading scenes with a possible transition.
/// Link to documentation: N.A.
/// </summary>
public class SceneManagerController : MonoBehaviour
{
    public static SceneManagerController Instance { get; private set; }

    [Header("Transition Settings")]
    public Color FadeColor = Color.black;
    public float FadeDuration = 1f;

    [SerializeField] private Canvas _fadeCanvas;
    [SerializeField] private Image _fadeImage;

    private void Awake()
    {
        //Singleton
        if (Instance == null)
        {
            Instance = this;
            StartCoroutine(Fade(0f));
            //DontDestroyOnLoad(gameObject);
            //CreateFadeCanvas();
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        //if (Input.anyKeyDown)
        //{
        //    LoadScene(SceneManager.GetActiveScene().name, true, true);
        //}
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    //Dynamically created canvas and image object that won't be destroy on scene switching.
    /*private void CreateFadeCanvas()
    {
        GameObject canvasObject = new GameObject("FadeCanvas");
        _fadeCanvas = canvasObject.AddComponent<Canvas>();
        _fadeCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        _fadeCanvas.sortingOrder = 1000;

        canvasObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

        GameObject imageObj = new GameObject("FadeImage");
        imageObj.transform.SetParent(canvasObject.transform, false);
        _fadeImage = imageObj.AddComponent<Image>();
        _fadeImage.color = new Color(FadeColor.r, FadeColor.g, FadeColor.b, 0);

        RectTransform rectTransform = _fadeImage.rectTransform;
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        DontDestroyOnLoad(canvasObject);
    }*/

    private IEnumerator Fade(float targetAlpha)
    {
        Color color = _fadeImage.color;
        float startAlpha = color.a;
        float time = 0;

        while (time < FadeDuration)
        {
            time += Time.deltaTime;
            float t = time / FadeDuration;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            _fadeImage.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        _fadeImage.color = color;

    }

    public IEnumerator LoadSceneAsync(string sceneName, bool showFade = true)
    {
        if (showFade)
        {
            yield return StartCoroutine(Fade(1f));
        }

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        if (showFade)
        {
            yield return StartCoroutine(Fade(0f));
        }
    }

    public void LoadScene(string sceneName, bool useAsync = true, bool showFade = true)
    {
        if (useAsync)
        {
            StartCoroutine(LoadSceneAsync(sceneName, showFade));
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    public void ReloadCurrentScene(bool useAsync = true, bool showFade = true)
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        LoadScene(currentSceneName, useAsync, showFade);
    }

    public void GetScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            LoadScene(sceneName);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
