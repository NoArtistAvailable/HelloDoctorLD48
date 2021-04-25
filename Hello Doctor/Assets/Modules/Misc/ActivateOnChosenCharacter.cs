using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOnChosenCharacter : MonoBehaviour
{
    public DiffcultySetting character;
    // Start is called before the first frame update
    void Start()
    {
        if (!GameDifficulty.Instance) return;
        if (GameDifficulty.Instance.setting == character) GetComponent<Renderer>().enabled = true;
    }
}
