using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingWaypoint : MonoBehaviour
{
    [SerializeField] PatrollingWaypoint next;
    [SerializeField] Transform wayPointTransform;
    [SerializeField] float debugArrowAngle = 30f;
    [SerializeField] float debugArrowSize = 2f;
    internal Vector3 GetPosition()
    {
        return wayPointTransform.position;
    }

    internal PatrollingWaypoint Next()
    {
        return next;
    }

    private void Start()
    {
        wayPointTransform.gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        if (next != null)
        {   Gizmos.color = Color.yellow;
            Vector3 nextPos = next.GetPosition();
            Gizmos.DrawLine(GetPosition(), nextPos);
            Vector3 dirToNext = -(nextPos - GetPosition()).normalized;
            Vector3 leftDir = Quaternion.AngleAxis(debugArrowAngle, Vector3.up) * dirToNext * debugArrowSize;
            Vector3 rightDir = Quaternion.AngleAxis(-debugArrowAngle, Vector3.up) * dirToNext * debugArrowSize;
            Gizmos.DrawRay(nextPos, leftDir);
            Gizmos.DrawRay(nextPos, rightDir);
        }
    }
}
