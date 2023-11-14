using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTTask_ClearBlackboardEntry : BTNode
{
    [SerializeField] string keyName;

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

        blackboard.ClearBlackboardData(keyName);
        return BTNodeResult.Success;
    }
}
