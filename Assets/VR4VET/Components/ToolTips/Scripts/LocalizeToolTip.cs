using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

public class LocalizeToolTip : MonoBehaviour
{

    [SerializeField] private LocalizedStringTable stringTable;
    [SerializeField] private string entryKeyHeader;
    [SerializeField] private string entryKeyText;

    private void Start()
    {
        if (stringTable.IsEmpty) 
        {
            Debug.LogError("StringTable is not set");
            return;
        }

        if (string.IsNullOrEmpty(entryKeyText))
        {
            Debug.LogError("EntryKeyText is not set");
            return;
        }
    }

    void OnEnable() {
        stringTable.TableChanged += LoadStrings;
    }

    void OnDisable() {
        stringTable.TableChanged -= LoadStrings;
    }

    void LoadStrings(StringTable stringTable) {
        if (!string.IsNullOrEmpty(entryKeyHeader)) {
            UpdateHeader(stringTable.GetEntry(entryKeyHeader).GetLocalizedString());
        }
        UpdateTextContent(stringTable.GetEntry(entryKeyText).GetLocalizedString());
    }

    private void UpdateTextContent(string translatedString) {
        GetComponent<TooltipScript>().TextContent = translatedString;
    }

    private void UpdateHeader(string translatedString) {
        GetComponent<TooltipScript>().Header = translatedString;
    }

}
