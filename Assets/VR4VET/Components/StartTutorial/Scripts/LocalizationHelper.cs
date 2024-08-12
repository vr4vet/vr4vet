using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

public class LocalizationHelper : MonoBehaviour
{
    [Serializable]
    public class Entry
    {
        public string entryKey;
        public TextMeshProUGUI textContent;
    }

    [SerializeField] private LocalizedStringTable stringTable;
    [SerializeField] public List<Entry> entries;

    void OnEnable()
    {
        stringTable.TableChanged += LoadStrings;
    }

    void OnDisable()
    {
        stringTable.TableChanged -= LoadStrings;
    }

    void LoadStrings(StringTable stringTable)
    {
        foreach (Entry entry in entries) 
        {
            TextMeshProUGUI textContent = entry.textContent;
            textContent.text = stringTable.GetEntry(entry.entryKey).GetLocalizedString();
        }
    }
}
