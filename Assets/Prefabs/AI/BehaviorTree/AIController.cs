using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] BehaviorTree behaviorTreeAsset;
    BehaviorTree behaviorTree;
    PerceptionComponent perceptionComponent;

    private void Awake()
    {
        perceptionComponent = GetComponent<PerceptionComponent>();
        if (perceptionComponent)
        {
            perceptionComponent.onTargetUpdated += TargetUpdated;
        }
    }

    private void TargetUpdated(GameObject newTarget)
    {
        if(newTarget == null)
        {
            if(behaviorTree.GetBlackboard().GetBlackboardData("target", out GameObject target))
            {
                behaviorTree.GetBlackboard().SetBlackboardData("lastSeenLocation", target.transform.position);
            }
        }

        behaviorTree.GetBlackboard().SetBlackboardData("target", newTarget);
    }

    // Start is called before the first frame update
    void Start()
    {
        behaviorTree = behaviorTreeAsset.CloneTree();
        behaviorTree?.PreConstruct();
        behaviorTree.GetBlackboard().SetBlackboardData("owner", gameObject);

        behaviorTree.Start();
    }

    public BehaviorTree GetBehaviorTree()
    {
        if(behaviorTree)
        {
            return behaviorTree;
        }
        else
        {
            return behaviorTreeAsset;
        }
    }

    // Update is called once per frame
    void Update()
    {
        behaviorTree?.Update();
    }

    internal void StopAILogic()
    {
        behaviorTree.Stop();
    }
}
