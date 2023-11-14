using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTTask_MoveToGameObject : BTNode
{
    [SerializeField] string targetKeyName = "target";

    [SerializeField] float acceptableDistance = 2f;

    NavMeshAgent agent;

    GameObject owner;
    GameObject target;

    Blackboard blackboard;
    protected override BTNodeResult Execute()
    {
        if(!GetBehaviorTree())
        {
            return BTNodeResult.Failure;
        }
        blackboard = GetBehaviorTree().GetBlackboard();
        if(blackboard == null)
        {
            return BTNodeResult.Failure;
        }
        if(!blackboard.GetBlackboardData("owner", out owner))
        {
            return BTNodeResult.Failure;
        }

        agent = owner.GetComponent<NavMeshAgent>();

        if(!agent) 
        {
            return BTNodeResult.Failure; 
        }
        if (!blackboard.GetBlackboardData(targetKeyName, out target))
        {
            return BTNodeResult.Failure;
        }
        blackboard.onblackboardValueChanged -= BlackboardValueChanged; 
        blackboard.onblackboardValueChanged += BlackboardValueChanged; //keep our value up to date

        agent.stoppingDistance = acceptableDistance;
        agent.SetDestination(target.transform.position);
        agent.isStopped = false;

        return BTNodeResult.InProgress;
    }

    private void BlackboardValueChanged(BlackboardEntry entry)
    {
        if(entry.GetKeyName() == targetKeyName) //if target updated, update target variable
        {
            entry.GetValue(out target);
        }
    }

    protected override BTNodeResult Update()
    {
        if(target == null)
        {
            return BTNodeResult.Failure;
        }

        if(InAcceptableDistance())
        {
            return BTNodeResult.Success;
        }
        agent.SetDestination(target.transform.position);

        return BTNodeResult.InProgress;
    }

    private bool InAcceptableDistance()
    {
        return Vector3.Distance(owner.transform.position, target.transform.position) <= acceptableDistance;
    }

    public override void End()
    {
        if(agent != null) { 
            agent.isStopped = true;
        }
        if (blackboard)
        {
            blackboard.onblackboardValueChanged -= BlackboardValueChanged;
        }
        base.End();
    }
}
