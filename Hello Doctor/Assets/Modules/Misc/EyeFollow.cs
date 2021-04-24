using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeFollow : MonoBehaviour
{
    public Vector2 constraint = Vector2.one;
    public Transform toFollow;
    public Transform followFrom;

    Vector3 localStartPos;
    Vector3 startPos;

    public float behindDistance = 10f;

    public Vector3 localTargetPosition;

    public EyeFollow otherEye;

    private void Start()
    {
        localStartPos = transform.localPosition;
        startPos = transform.position;
    }

    private void Update()
    {
        Plane plane = new Plane(transform.up, startPos);
        Debug.DrawRay(startPos, transform.up);
        //Debug.DrawLine(followFrom.position, toFollow.position);
        Vector3 direction = toFollow.position - followFrom.position;
        direction = (toFollow.position + direction.normalized * behindDistance) - followFrom.position;
        Ray ray = new Ray(followFrom.position, direction.normalized);
        if(plane.Raycast(ray,out float enter))
        {
            Debug.DrawRay(followFrom.position, direction.normalized * enter);
            Vector3 targetPos = ray.GetPoint(enter);
            Vector3 localTargetPos = transform.parent.InverseTransformPoint(targetPos);
            Vector3 difference = localTargetPos - localStartPos;
            difference.x = Mathf.Clamp(difference.x, -constraint.x, constraint.x);
            difference.y = Mathf.Clamp(difference.y, -constraint.y, constraint.y);
            difference.z = 0f;
            localTargetPosition = difference;
        }
    }

    private void LateUpdate()
    {
        transform.localPosition = localStartPos + (otherEye.localTargetPosition + localTargetPosition) / 2f;
    }

}
