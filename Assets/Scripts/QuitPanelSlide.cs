using UnityEngine;
using System.Collections;

public class QuitPanelSlide : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private RectTransform quitPanel;

    [Header("Slide Settings (Customizable)")]
    [SerializeField] private float slideInDuration = 0.4f;
    [SerializeField] private float slideOutDuration = 0.3f;
    [SerializeField] private AnimationCurve easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Vector2 onScreenPos;
    private Vector2 offScreenPos;
    private Coroutine slideRoutine;

    void Awake()
    {
        onScreenPos = quitPanel.anchoredPosition;
        offScreenPos = new Vector2(onScreenPos.x, onScreenPos.y - quitPanel.rect.height - Mathf.Abs(onScreenPos.y));

        quitPanel.anchoredPosition = offScreenPos;
        quitPanel.gameObject.SetActive(false); // fully hidden, not just off-screen
    }

    // Call this from the Quit Button's OnClick()
    public void OpenQuitPanel()
    {
        quitPanel.gameObject.SetActive(true);
        quitPanel.anchoredPosition = offScreenPos; // reset position every time it opens

        if (slideRoutine != null) StopCoroutine(slideRoutine);
        slideRoutine = StartCoroutine(Slide(offScreenPos, onScreenPos, slideInDuration, closeAfter: false));
    }

    // Call this from the "No" Button's OnClick()
    public void CloseQuitPanel()
    {
        if (slideRoutine != null) StopCoroutine(slideRoutine);
        slideRoutine = StartCoroutine(Slide(onScreenPos, offScreenPos, slideOutDuration, closeAfter: true));
    }

    // Call this from the "Yes" Button's OnClick()
    public void ConfirmQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private IEnumerator Slide(Vector2 from, Vector2 to, float duration, bool closeAfter)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = easeCurve.Evaluate(elapsed / duration);
            quitPanel.anchoredPosition = Vector2.Lerp(from, to, t);
            yield return null;
        }
        quitPanel.anchoredPosition = to;

        if (closeAfter) quitPanel.gameObject.SetActive(false); // fully deactivate once slid away
    }
}