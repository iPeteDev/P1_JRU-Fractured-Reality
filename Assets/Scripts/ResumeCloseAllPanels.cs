using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResumeCloseAllPanels : MonoBehaviour
{
    [System.Serializable]
    public class ClosablePanel
    {
        public string panelName;      // just for readability
        public RectTransform panel;
        public bool slideFromLeft = true; // true = off-screen left, false = off-screen right
        [HideInInspector] public Vector2 onScreenPos;
        [HideInInspector] public Vector2 offScreenPos;
    }

    [Header("Panels to Close on Resume (drag any panel here)")]
    [SerializeField] private List<ClosablePanel> panels;

    [Header("Dim Overlay")]
    [SerializeField] private GameObject dimOverlay;

    [Header("Slide Settings")]
    [SerializeField] private float slideOutDuration = 0.3f;
    [SerializeField] private AnimationCurve easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    void Start()
    {
        foreach (var p in panels)
        {
            p.onScreenPos = p.panel.anchoredPosition;
            float dir = p.slideFromLeft ? -1f : 1f;
            p.offScreenPos = new Vector2(
                p.onScreenPos.x + dir * (p.panel.rect.width + Mathf.Abs(p.onScreenPos.x)),
                p.onScreenPos.y
            );
        }
    }

    // Call this from the Resume Button's OnClick()
    public void CloseAll()
    {
        StartCoroutine(CloseAllRoutine());
    }

    private IEnumerator CloseAllRoutine()
    {
        List<Coroutine> running = new List<Coroutine>();

        foreach (var p in panels)
        {
            running.Add(StartCoroutine(Slide(p.panel, p.panel.anchoredPosition, p.offScreenPos, slideOutDuration)));
        }

        // wait for the longest one to finish
        foreach (var r in running) yield return r;

        if (dimOverlay != null) dimOverlay.SetActive(false);
        Time.timeScale = 1f;
    }

    private IEnumerator Slide(RectTransform rt, Vector2 from, Vector2 to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = easeCurve.Evaluate(elapsed / duration);
            rt.anchoredPosition = Vector2.Lerp(from, to, t);
            yield return null;
        }
        rt.anchoredPosition = to;
    }
}