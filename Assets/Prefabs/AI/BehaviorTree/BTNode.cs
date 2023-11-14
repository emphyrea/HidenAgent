using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum BTNodeResult
{ 
    Success,
    InProgress,
    Failure
}

public enum BTNodePortType
{
    None,
    Single,
    Multi
}

public abstract class BTNode : ScriptableObject
{
    [HideInInspector]
    [SerializeField] Vector2 graphPos;

    public delegate void OnNodeStateChange(BTNodeResult newState);
    public event OnNodeStateChange onNodeStateChanged;
    bool isStarted = false;

    [HideInInspector]
    [SerializeField] string guid = "";

    [SerializeField] int priority;

    BehaviorTree owningBehaviorTree;
    internal Action<BTNode> onBecomeActive;

    public int GetPriority()
    {
        return priority;
    }
    public void SortPriority(ref int priorityCount)
    {
        priority = priorityCount++;
    }
    public Blackboard GetBlackboard()
    {
        if(GetBehaviorTree())
        {
            return GetBehaviorTree().GetBlackboard();
        }
        return null;
    }

    public GameObject GetOwner()
    {
        if(GetBlackboard())
        {
            GetBlackboard().GetBlackboardData("owner", out GameObject owner);
            return owner;
        }
        return null;
    }

    public IBTTaskInterface GetInterface()
    {
        GameObject owner = GetOwner();
        if(owner)
        {
            return owner.GetComponent<IBTTaskInterface>();
        }
        return null;
    }

    public void Init(BehaviorTree behaviorTree)
    {
        owningBehaviorTree = behaviorTree;
    }

    public BehaviorTree GetBehaviorTree() { return owningBehaviorTree; }

    public virtual BTNodePortType GetInputPortType()
    {
        return BTNodePortType.Single;
    }

    public virtual BTNodePortType GetOutputPortType() 
    { 
        return BTNodePortType.None; 
    }

    //UpdateNode will be called by an Update function in a monobehavior
    public BTNodeResult UpdateNode()
    {
        if(!isStarted)
        {
            onBecomeActive?.Invoke(this);
            BTNodeResult executeResult = Execute();
            onNodeStateChanged?.Invoke(executeResult);
            isStarted = true;
            //if not in progress, we have either failed or succeeded
            if (executeResult != BTNodeResult.InProgress)
            {
                End();
                return executeResult;
            }
        }

        BTNodeResult updateResult = Update();
        onNodeStateChanged?.Invoke(updateResult);

        if(updateResult != BTNodeResult.InProgress)
        {
            End();
        }
        
        return updateResult;
 
    }

    public virtual void End()
    {
        isStarted = false;
    }

    protected virtual BTNodeResult Update()
    {
        return BTNodeResult.Success;
    }

    protected virtual BTNodeResult Execute()
    {
        return BTNodeResult.Success;
    }

    public void SetGraphPosition(Vector2 newPos)
    {
        graphPos = newPos;
    }


    public Vector2 GetGraphPosition()
    {
        return graphPos;
    }

    public virtual bool Contains(BTNode node)
    {
        return this == node;
    }

    public string GetGUID()
    {
        if (guid == "")
        {
            guid = GUID.Generate().ToString();
        }

        return guid;
    }


    public virtual BTNode CloneNode()
    {
        return Instantiate(this);
    }
}
