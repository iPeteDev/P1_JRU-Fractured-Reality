using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreenManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider progressBar;
    [SerializeField] private TMPro.TMP_Text progressText;

    [Header("Load Duration (seconds)")]
    [SerializeField] private float minLoadTime = 5f;
    [SerializeField] private float maxLoadTime = 10f;

    void Start()
    {
        Time.timeScale = 1f; // safety reset in case a previous pause never resumed
        StartCoroutine(LoadTargetScene());
    }

    private IEnumerator LoadTargetScene()
    {
        string sceneToLoad = StorySceneLoader.targetSceneName;

        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogError("LoadingScreenManager: No target scene was set!");
            yield break;
        }

        float targetDuration = Random.Range(minLoadTime, maxLoadTime);
        float elapsed = 0f;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        operation.allowSceneActivation = false;

        while (elapsed < targetDuration)
        {
            elapsed += Time.unscaledDeltaTime; // immune to timeScale
            float t = Mathf.Clamp01(elapsed / targetDuration);
            UpdateUI(t);
            yield return null;
        }

        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        UpdateUI(1f);
        yield return new WaitForSecondsRealtime(0.25f); // also timeScale-safe

        operation.allowSceneActivation = true;

        while (!operation.isDone)
        {
            yield return null;
        }
    }

    private void UpdateUI(float t)
    {
        if (progressBar != null) progressBar.value = t;
        if (progressText != null) progressText.text = Mathf.RoundToInt(t * 100f) + "%";
    }
}