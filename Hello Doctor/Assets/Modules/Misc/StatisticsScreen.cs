using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatisticsScreen : MonoBehaviour
{
    public Text patientNameField, doctorNameField, stressField, clicksField, succeededField;

    private void OnEnable()
    {
        clicksField.text = CardGame.Clicks.ToString();
        succeededField.text = MineSweeperGame.Instance.success ? "SUCCESS" : "FAILURE";
        stressField.text = CardGame.Stress.ToString();
        patientNameField.text = GameDifficulty.Instance.setting.name;

        if (PlayerPrefs.HasKey("PlayerName")) doctorNameField.text = "Dr. " + PlayerPrefs.GetString("PlayerName");

        if (MineSweeperGame.Instance.success)
        {
            if(!PlayerPrefs.HasKey(GameDifficulty.Instance.setting.name + "_Stress") || PlayerPrefs.GetInt(GameDifficulty.Instance.setting.name + "_Stress") > CardGame.Stress)
                PlayerPrefs.SetInt(GameDifficulty.Instance.setting.name + "_Stress", CardGame.Stress);
            if (!PlayerPrefs.HasKey(GameDifficulty.Instance.setting.name + "_Clicks") || PlayerPrefs.GetInt(GameDifficulty.Instance.setting.name + "_Clicks") > CardGame.Clicks)
                PlayerPrefs.SetInt(GameDifficulty.Instance.setting.name + "_Clicks", CardGame.Clicks);
        }
    }
}
