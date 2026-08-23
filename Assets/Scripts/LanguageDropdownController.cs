using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System.Collections;
using System.Collections.Generic;

public class LanguageDropdownController : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Dropdown languageDropdown;

    private List<Locale> availableLocales;

    // Map locale codes to short display names
    private string GetShortName(Locale locale)
    {
        string code = locale.Identifier.Code;
        if (code.StartsWith("en")) return "English";
        if (code.StartsWith("fil")) return "Filipino";
        return locale.LocaleName; // fallback for anything else
    }

    private IEnumerator Start()
    {
        yield return LocalizationSettings.InitializationOperation;

        availableLocales = LocalizationSettings.AvailableLocales.Locales;

        languageDropdown.ClearOptions();
        List<string> options = new List<string>();
        foreach (var locale in availableLocales)
        {
            options.Add(GetShortName(locale));
        }
        languageDropdown.AddOptions(options);

        int currentIndex = availableLocales.IndexOf(LocalizationSettings.SelectedLocale);
        languageDropdown.value = currentIndex >= 0 ? currentIndex : 0;
        languageDropdown.RefreshShownValue();

        languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
    }

    private void OnLanguageChanged(int index)
    {
        LocalizationSettings.SelectedLocale = availableLocales[index];
    }
}