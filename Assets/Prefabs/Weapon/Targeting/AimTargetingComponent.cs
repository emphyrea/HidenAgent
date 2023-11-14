using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimTargetingComponent : MonoBehaviour
{
    [SerializeField] Transform aimTransform;
    [SerializeField] float aimDistance;
    [SerializeField] LayerMask aimMask;
    public GameObject GetTarget()
    {
        RaycastHit hit;
        if (Physics.Raycast(aimTransform.position, GetAimDir(), out hit, aimDistance, aimMask))
        {
            return hit.collider.gameObject;
        }
        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(aimTransform.position, aimTransform.position + aimTransform.forward * aimDistance);
    }

    Vector3 GetAimDir()
    {
        return new Vector3(aimTransform.forward.x, 0f, aimTransform.forward.z);
    }
}
