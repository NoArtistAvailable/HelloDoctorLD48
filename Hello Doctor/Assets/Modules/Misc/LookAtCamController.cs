using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamController : MonoBehaviour
{
    static LookAtCamController _instance;
    public static LookAtCamController Instance { get { if (!_instance) _instance = FindObjectOfType<LookAtCamController>(); return _instance; } }

    static Camera _cam;
    static Camera Cam { get { if(!_cam) _cam = Camera.main; return _cam; } }

    List<LookAtCamAngles> toUpdate = new List<LookAtCamAngles>();

    private void Update()
    {
        Quaternion lookToCam = Quaternion.LookRotation((Cam.transform.position - transform.position).normalized);
        foreach(var obj in toUpdate)
        {
            obj.transform.rotation = lookToCam;
        }
    }

    public static void Register(LookAtCamAngles angles)
    {
        Instance.toUpdate.Add(angles);
    }

    public static void Unregister(LookAtCamAngles angles)
    {
        if (!Instance) return;
        Instance.toUpdate.Remove(angles);
    }
}
