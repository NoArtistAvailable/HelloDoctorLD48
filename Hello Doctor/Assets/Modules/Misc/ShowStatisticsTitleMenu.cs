using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowStatisticsTitleMenu : MonoBehaviour
{
    public DiffcultySetting character;
    public Text stressField, clicksField;

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey(character.name + "_Stress"))
        {
            stressField.text = PlayerPrefs.GetInt(character.name + "_Stress").ToString();
            clicksField.text = PlayerPrefs.GetInt(character.name + "_Clicks").ToString();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

}
