using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{

    public Vector2 camSpeed = Vector2.one * 5f;
    public Vector2 yRange = new Vector2(-90f, 90f);
    Vector3 rotation;
    Vector3 mousePrevious;

    private void OnEnable()
    {
        mousePrevious = Input.mousePosition;
        rotation = transform.localEulerAngles;
    }

    private void OnApplicationFocus(bool focus)
    {
        mousePrevious = Input.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        var mouseDelta = Input.mousePosition - mousePrevious;
        mouseDelta /= Screen.width;
        mousePrevious = Input.mousePosition;
        if (Input.GetMouseButton(1))
        {
            rotation.x += mouseDelta.y * camSpeed.y;
            rotation.y += mouseDelta.x * camSpeed.x;
            rotation.x = Mathf.Clamp(rotation.x, yRange.x, yRange.y);
            transform.rotation = Quaternion.Euler(rotation);
        }
    }
}
