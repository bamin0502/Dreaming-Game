using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAt : MonoBehaviour
{
    public Transform target;
    public float speed = 5.0f;
    public Vector3 offset = new Vector3(0, 6f, -10.0f);

    private void Update()
    {
        if (!target)
            return;

        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
        transform.LookAt(target);
    }
}
