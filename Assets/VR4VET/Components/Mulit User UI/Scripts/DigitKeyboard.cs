using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DigitKeyboard : MonoBehaviour
{

    public List<GameObject> displayDigits;
    public GameObject networkManager;
    
    private int digitsPressed = 0;
    private string codeEntered = "";

    public void digitPressed(int digit)
    {
        if (digitsPressed >= 5) { return; }
        codeEntered += digit.ToString();
        GameObject displayDigit = displayDigits[digitsPressed];

        displayDigit.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = digit.ToString();

        if (digitsPressed >= 4) {
            digitsPressed++;
            return; 
        }
        displayDigit.transform.Find("Selected Image").gameObject.SetActive(false);
        displayDigits[digitsPressed + 1].transform.Find("Selected Image").gameObject.SetActive(true);

        digitsPressed++;
    }

    public void backspacePressed ()
    {
        if (digitsPressed == 0) { return; }

        codeEntered = codeEntered.Remove(codeEntered.Length - 1);
        digitsPressed--;

        GameObject displayDigit = displayDigits[digitsPressed];

        displayDigit.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
        if (digitsPressed >= 4)
        {
            return;
        }
        displayDigit.transform.Find("Selected Image").gameObject.SetActive(true);
        displayDigits[digitsPressed + 1].transform.Find("Selected Image").gameObject.SetActive(false);
    }

    public void joinPressed()
    {
        if (digitsPressed < 5) { return; }
        networkManager.GetComponent<NetworkManager>().JoinRoom(codeEntered);
    }
}
