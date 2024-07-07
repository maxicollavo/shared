using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public Transform target;

    private void LateUpdate()
    {
        if (!target) return;
        FollowTarget();
    }

    void FollowTarget()
    {
        Vector3 pos = target.position;
        pos.z = transform.position.z;
        pos.y = transform.position.y;
        transform.position = pos;
    }
}
