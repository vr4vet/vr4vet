using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayName : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;

    void Awake()
    {
        nameText.text = name;
    }

}
