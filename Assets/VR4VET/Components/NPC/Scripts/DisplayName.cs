using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayName : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;

    void Awake()
    {
        _nameText.text = name;
    }

    public void UpdateDisplayedName(string name) {
        this.name = name;
        _nameText.text = name;
    }

}
