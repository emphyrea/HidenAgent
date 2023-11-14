using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.UI.GridLayoutGroup;

public class BTTask_Rotate : BTNode
{
    [SerializeField] float angle;
    [SerializeField] float acceptableOffset;
    [SerializeField] float turnSpeed;

    GameObject owner;

    Quaternion goalRot;

    protected override BTNodeResult Execute()
    {
        if (!GetBehaviorTree())
        {
            return BTNodeResult.Failure;
        }
        Blackboard blackboard = GetBehaviorTree().GetBlackboard();
        if (!blackboard)
        {
            return BTNodeResult.Failure;
        }
        if (!blackboard.GetBlackboardData("owner", out owner))
        {
            return BTNodeResult.Failure;
        }

        goalRot = Quaternion.AngleAxis(angle, Vector3.up) * owner.transform.rotation;
        if(InAcceptableAngleRange())
        {
            return BTNodeResult.Success;
        }
        return BTNodeResult.InProgress;
    }

    private bool InAcceptableAngleRange()
    {
        return Quaternion.Angle(goalRot, owner.transform.rotation) < acceptableOffset;
    }

    protected override BTNodeResult Update()
    {
        owner.transform.rotation = Quaternion.Lerp(owner.transform.rotation, goalRot, Time.deltaTime * turnSpeed);
        if(InAcceptableAngleRange())
        {
            return BTNodeResult.Success;
        }
        return BTNodeResult.InProgress;
    }
}
