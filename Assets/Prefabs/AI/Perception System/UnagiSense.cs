using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Perception/UnagiSense")]
public class UnagiSense : Sense
{
    [SerializeField] float range = 2f;
    public override bool IsStimuliSensable(PerceptionStimuli stimuli)
    {
        return Vector3.Distance(Owner.transform.position, stimuli.transform.position) <= range;
    }

    public override void DrawDebug()
    {
        base.DrawDebug();
        if (Owner)
        {
            Gizmos.DrawWireSphere(Owner.transform.position, range);
        }
    }
}
