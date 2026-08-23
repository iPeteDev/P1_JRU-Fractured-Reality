using UnityEngine;
using System.Collections;

public class PausePanelSlide : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject dimOverlay;
    [SerializeField] private RectTransform pausePanel;
    [SerializeField] private CanvasGroup pausePanelGroup; // optional — leave empty if not using fade

    [Header("Linked Sub-Panels")]
    [SerializeField] private PanelSlideManager subPanelManager; // e.g. Settings inside Pause menu

    [Header("Slide Settings (Customizable)")]
    [SerializeField] private float slideInDuration = 0.4f;
    [SerializeField] private float slideOutDuration = 0.3f;
    [SerializeField] private AnimationCurve easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Vector2 onScreenPos;
    private Vector2 offScreenPos;
    private Coroutine slideRoutine;
    private bool isPaused = false;

    void Start()
    {
        onScreenPos = pausePanel.anchoredPosition;
        offScreenPos = new Vector2(onScreenPos.x - pausePanel.rect.width - Mathf.Abs(onScreenPos.x), onScreenPos.y);

        pausePanel.anchoredPosition = offScreenPos;
        if (pausePanelGroup != null) pausePanelGroup.alpha = 0f;
        if (dimOverlay != null) dimOverlay.SetActive(false);
    }

    // Call this from the Pause Button's OnClick()
    public void TogglePause()
    {
        if (isPaused) ClosePause();
        else OpenPause();
    }

    public void OpenPause()
    {
        isPaused = true;
        if (dimOverlay != null) dimOverlay.SetActive(true);

        if (slideRoutine != null) StopCoroutine(slideRoutine);
        slideRoutine = StartCoroutine(SlideInRoutine());
    }

    // Call this from the Resume Button's OnClick()
    public void ClosePause()
    {
        isPaused = false;

        // Close any sub-panel (Settings etc.) currently open inside the pause menu
        if (subPanelManager != null) subPanelManager.CloseAllPanels();

        if (slideRoutine != null) StopCoroutine(slideRoutine);
        slideRoutine = StartCoroutine(SlideOutRoutine());
    }

    private IEnumerator SlideInRoutine()
    {
        yield return StartCoroutine(Slide(offScreenPos, onScreenPos, 0f, 1f, slideInDuration, unscaledTime: true));
        Time.timeScale = 0f;
    }

    private IEnumerator SlideOutRoutine()
    {
        yield return StartCoroutine(Slide(onScreenPos, offScreenPos, 1f, 0f, slideOutDuration, unscaledTime: true));
        if (dimOverlay != null) dimOverlay.SetActive(false);
        Time.timeScale = 1f;
    }

    private IEnumerator Slide(Vector2 fromPos, Vector2 toPos, float fromAlpha, float toAlpha, float duration, bool unscaledTime = false)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            float t = easeCurve.Evaluate(elapsed / duration);

            pausePanel.anchoredPosition = Vector2.Lerp(fromPos, toPos, t);
            if (pausePanelGroup != null) pausePanelGroup.alpha = Mathf.Lerp(fromAlpha, toAlpha, t);

            yield return null;
        }
        pausePanel.anchoredPosition = toPos;
        if (pausePanelGroup != null) pausePanelGroup.alpha = toAlpha;
    }
}