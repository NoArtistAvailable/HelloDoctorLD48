using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AskForPlayerName : MonoBehaviour
{
    public InputField inputField;

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            gameObject.SetActive(false);
            return;
        }
    }

    public void SetName()
    {
        PlayerPrefs.SetString("PlayerName", inputField.text);
        gameObject.SetActive(false);
    }
}
