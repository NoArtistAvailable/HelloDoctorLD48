using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnableRelay : MonoBehaviour
{
    public UnityEvent<bool> OnEnabled;

    private void OnEnable()
    {
        OnEnabled.Invoke(true);
    }

    private void OnDisable()
    {
        OnEnabled.Invoke(false);
    }
}
