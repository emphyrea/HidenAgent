using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTTask_GetNextPatrolPoint : BTNode
{
    [SerializeField] string patrolPointKeyName;
    protected override BTNodeResult Execute()
    {
        if(!GetBehaviorTree())
        {
            return BTNodeResult.Failure;
        }

        Blackboard blackboard = GetBehaviorTree().GetBlackboard();

        if(!blackboard)
        {
            return BTNodeResult.Failure;
        }

        if(!blackboard.GetBlackboardData("owner", out GameObject owner))
        {
            return BTNodeResult.Failure;
        }
        PatrollingComponent patrollingComponent = owner.GetComponent<PatrollingComponent>();
        if (!patrollingComponent || !patrollingComponent.GetNextPatrollingPosition(out Vector3 pos))
        {
            return BTNodeResult.Failure;
        }
        blackboard.SetBlackboardData(patrolPointKeyName, pos);
        return BTNodeResult.Success;

    }
}
