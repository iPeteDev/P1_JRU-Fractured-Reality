using UnityEngine;
using UnityEngine.SceneManagement;

public class StorySceneLoader : MonoBehaviour
{
    // Holds the name of the in-game scene to load — set directly by the Start button
    public static string targetSceneName;

    // Call this directly from the Start button's OnClick(), passing the in-game scene name
    public void PressStart(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("StorySceneLoader: No scene name passed to PressStart!");
            return;
        }

        targetSceneName = sceneName;
        SceneManager.LoadScene("Loading Screen");
    }
}