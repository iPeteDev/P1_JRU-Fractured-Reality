using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PanelSlideManager : MonoBehaviour
{
    [System.Serializable]
    public class SlidePanel
    {
        public string panelName;      // just for readability in Inspector
        public RectTransform panel;
        [HideInInspector] public Vector2 onScreenPos;
        [HideInInspector] public Vector2 offScreenPos;
    }

    [Header("Panels (Play, Load, Settings, Help)")]
    [SerializeField] private List<SlidePanel> panels;

    [Header("Logo Animation Object")]
    [SerializeField] private GameObject logoAnimationObject;

    [Header("Slide Speed Settings (Customizable)")]
    [SerializeField] private float slideInDuration = 0.4f;
    [SerializeField] private float slideOutDuration = 0.3f;
    [SerializeField] private AnimationCurve easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private SlidePanel currentOpenPanel;
    private Coroutine slideRoutine;

    void Start()
    {
        foreach (var p in panels)
        {
            p.onScreenPos = p.panel.anchoredPosition;
            p.offScreenPos = new Vector2(
                p.onScreenPos.x + p.panel.rect.width + Mathf.Abs(p.onScreenPos.x),
                p.onScreenPos.y
            );
            p.panel.anchoredPosition = p.offScreenPos; // start hidden
        }
    }

    // Call this from each Button's OnClick(), passing a different index per button
    public void ShowPanel(int index)
    {
        if (index < 0 || index >= panels.Count) return;

        SlidePanel target = panels[index];

        if (logoAnimationObject != null && logoAnimationObject.activeSelf)
        {
            logoAnimationObject.SetActive(false);
        }

        if (currentOpenPanel == target) return;

        if (slideRoutine != null) StopCoroutine(slideRoutine);
        slideRoutine = StartCoroutine(SwitchPanels(target));
    }

    // Call this from a Resume/Close button to close whatever panel is currently open
    public void CloseAllPanels()
    {
        if (currentOpenPanel == null) return;

        if (slideRoutine != null) StopCoroutine(slideRoutine);
        slideRoutine = StartCoroutine(CloseCurrentPanel());
    }

    private IEnumerator CloseCurrentPanel()
    {
        yield return StartCoroutine(Slide(currentOpenPanel.panel, currentOpenPanel.panel.anchoredPosition, currentOpenPanel.offScreenPos, slideOutDuration));
        currentOpenPanel = null;
    }

    private IEnumerator SwitchPanels(SlidePanel target)
    {
        if (currentOpenPanel != null)
        {
            yield return StartCoroutine(Slide(currentOpenPanel.panel, currentOpenPanel.panel.anchoredPosition, currentOpenPanel.offScreenPos, slideOutDuration));
        }

        currentOpenPanel = target;
        yield return StartCoroutine(Slide(target.panel, target.panel.anchoredPosition, target.onScreenPos, slideInDuration));
    }

    private IEnumerator Slide(RectTransform rt, Vector2 from, Vector2 to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime; // works even while Time.timeScale = 0
            float t = easeCurve.Evaluate(elapsed / duration);
            rt.anchoredPosition = Vector2.Lerp(from, to, t);
            yield return null;
        }
        rt.anchoredPosition = to;
    }
}