using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIIndicator : MonoBehaviour
{
    public void GoTo(RectTransform target)
    {
        transform.position = new Vector3(transform.position.x, target.position.y, transform.position.z);
    }
}
