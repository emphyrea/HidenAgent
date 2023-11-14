using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Perception/SightSense")]
public class SightSense : Sense
{
    [SerializeField] float sightRange = 4f;
    [SerializeField] float eyeHeight = 1f;
    [SerializeField] float halfPeripheralAngle = 45f;

    public override bool IsStimuliSensable(PerceptionStimuli stimuli)
    {
        if (Vector3.Distance(stimuli.transform.position, Owner.transform.position) > sightRange)
        {
            return false;
        }

        Vector3 stimDir = (stimuli.transform.position - Owner.transform.position).normalized;
        Vector3 ownerForward = Owner.transform.forward;

        if(Vector3.Angle(stimDir, ownerForward) > halfPeripheralAngle)
        {
            return false;
        }

        if(Physics.Raycast(Owner.transform.position + Vector3.up * eyeHeight, stimDir, out RaycastHit hitInfo, sightRange))
        {
            if(hitInfo.collider.gameObject != stimuli.gameObject)
            {
                return false;
            }
        }

        return true;
    }

    public override void DrawDebug()
    {
        base.DrawDebug();
        if(Owner)
        {
            Vector3 drawCenter = Owner.transform.position + Vector3.up * eyeHeight;
            Gizmos.DrawWireSphere(drawCenter, sightRange);

            Vector3 leftDir = Quaternion.AngleAxis(halfPeripheralAngle, Vector3.up) * Owner.transform.forward;
            Vector3 rightDir = Quaternion.AngleAxis(-halfPeripheralAngle, Vector3.up) * Owner.transform.forward;

            Gizmos.DrawLine(drawCenter, drawCenter + leftDir * sightRange);
            Gizmos.DrawLine(drawCenter, drawCenter + rightDir * sightRange);
        }
    }

}
