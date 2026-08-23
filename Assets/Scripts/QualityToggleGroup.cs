using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the LOW / MEDIUM / HIGH quality buttons.
/// Only ONE stays highlighted at a time — pressing a new one turns off the old one.
/// Attach to an empty GameObject inside the Settings panel (e.g. "QualityGroup").
/// </summary>
public class QualityToggleGroup : MonoBehaviour
{
    [Header("Drag each Quality button's Image here (Low, Medium, High in order)")]
    [SerializeField] private Image lowHighlight;
    [SerializeField] private Image mediumHighlight;
    [SerializeField] private Image highHighlight;

    [Header("Colors")]
    [SerializeField] private Color highlightedColor = Color.white;
    [SerializeField] private Color normalColor = new Color(1f, 1f, 1f, 0.3f);

    private void Start()
    {
        SelectMedium(); // default starting quality
    }

    // Wire each button's OnClick() to the matching method below

    public void SelectLow()
    {
        SetHighlighted(lowHighlight, mediumHighlight, highHighlight);
    }

    public void SelectMedium()
    {
        SetHighlighted(mediumHighlight, lowHighlight, highHighlight);
    }

    public void SelectHigh()
    {
        SetHighlighted(highHighlight, lowHighlight, mediumHighlight);
    }

    private void SetHighlighted(Image active, Image other1, Image other2)
    {
        if (active != null) active.color = highlightedColor;
        if (other1 != null) other1.color = normalColor;
        if (other2 != null) other2.color = normalColor;
    }
}
