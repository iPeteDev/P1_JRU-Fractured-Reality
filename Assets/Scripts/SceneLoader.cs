using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Call this directly from any Button's OnClick(), passing the exact scene name as the string argument
    public void LoadSceneByName(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("SceneLoader: No scene name passed from the Button's OnClick()!");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }

    // Optional: load by Build Index instead
    public void LoadSceneByIndex(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }
}
