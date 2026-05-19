using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;
    public Vector3 offset = new Vector3(0, 2, -4);
    public float smoothSpeed = 0.125f;


    void FixedUpdate()
    {
        if (target.activeSelf == false) return;

        Vector3 desiredPosition = target.transform.position + offset;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target.transform);
    }
}