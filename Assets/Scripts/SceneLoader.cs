using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Tooltip("Exact name of the scene to load (must match the scene file name).")]
    [SerializeField] private string sceneToLoad;

    // Call this from the Button's OnClick() event
    public void LoadScene()
    {
        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogError("SceneLoader: No scene name assigned in the Inspector!");
            return;
        }

        SceneManager.LoadScene(sceneToLoad);
    }

    // Optional: use this instead if you want to load by Build Index
    public void LoadSceneByIndex(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }
}