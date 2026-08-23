using UnityEngine;
using TMPro;
using System.Collections;

public class InternetChecker : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private GameObject loadingIcon;
    [SerializeField] private GameObject refreshButton;

    [Header("Loading Icon Settings")]
    [SerializeField] private float rotationSpeed = 200f;

    [Header("Demo Settings")]
    [SerializeField] private float fakeLoadingDuration = 2f;

    private bool isChecking = false;

    void Awake()
    {
        Time.timeScale = 1f; // safety reset in case a previous pause never resumed
    }

    public void OnRefreshPressed()
    {
        if (isChecking) return;
        StartCoroutine(FakeCheckRoutine());
    }

    private IEnumerator FakeCheckRoutine()
    {
        isChecking = true;

        statusText.gameObject.SetActive(false);
        if (refreshButton != null) refreshButton.SetActive(false);
        loadingIcon.SetActive(true);

        Coroutine spin = StartCoroutine(SpinIcon());

        yield return new WaitForSecondsRealtime(fakeLoadingDuration); // timeScale-safe

        StopCoroutine(spin);
        loadingIcon.SetActive(false);
        if (refreshButton != null) refreshButton.SetActive(true);

        statusText.text = "No internet connection";
        statusText.gameObject.SetActive(true);

        isChecking = false;
    }

    private IEnumerator SpinIcon()
    {
        RectTransform rt = loadingIcon.GetComponent<RectTransform>();
        while (true)
        {
            rt.Rotate(0f, 0f, -rotationSpeed * Time.unscaledDeltaTime); // timeScale-safe
            yield return null;
        }
    }
}