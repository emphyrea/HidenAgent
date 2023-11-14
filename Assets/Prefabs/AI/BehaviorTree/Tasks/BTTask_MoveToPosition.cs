using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTTask_MoveToPosition : BTNode
{
    [SerializeField] string positionKeyName = "target";

    [SerializeField] float acceptableDistance = 2f;

    NavMeshAgent agent;

    GameObject owner;
    Vector3 pos;

    Blackboard blackboard;
    protected override BTNodeResult Execute()
    {
        if (!GetBehaviorTree())
        {
            return BTNodeResult.Failure;
        }
        blackboard = GetBehaviorTree().GetBlackboard();
        if (blackboard == null)
        {
            return BTNodeResult.Failure;
        }
        if (!blackboard.GetBlackboardData("owner", out owner))
        {
            return BTNodeResult.Failure;
        }

        agent = owner.GetComponent<NavMeshAgent>();

        if (!agent)
        {
            return BTNodeResult.Failure;
        }
        if (!blackboard.GetBlackboardData(positionKeyName, out pos))
        {
            return BTNodeResult.Failure;
        }

        agent.stoppingDistance = acceptableDistance;
        agent.SetDestination(pos);
        agent.isStopped = false;

        return BTNodeResult.InProgress;
    }

    private void BlackboardValueChanged(BlackboardEntry entry)
    {
        if (entry.GetKeyName() == positionKeyName) //if target updated, update target variable
        {
            entry.GetValue(out pos);
        }
    }

    protected override BTNodeResult Update()
    {

        if (InAcceptableDistance())
        {
            return BTNodeResult.Success;
        }

        return BTNodeResult.InProgress;
    }

    private bool InAcceptableDistance()
    {
        return Vector3.Distance(owner.transform.position, pos) <= acceptableDistance;
    }

    public override void End()
    {
        if (agent != null)
        {
            agent.isStopped = true;
        }
        base.End();
    }
}
