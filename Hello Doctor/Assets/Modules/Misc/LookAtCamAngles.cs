using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamAngles : MonoBehaviour
{
    private void OnEnable()
    {
        LookAtCamController.Register(this);
    }

    private void OnDisable()
    {
        LookAtCamController.Unregister(this);
    }
}
