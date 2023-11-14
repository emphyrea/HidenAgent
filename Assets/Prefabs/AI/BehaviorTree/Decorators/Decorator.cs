using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Decorator : BTNode, IBTNodeParent
{
    [HideInInspector] public BTNode child;

    public override BTNodePortType GetOutputPortType()
    {
        return BTNodePortType.Single;
    }

    protected override BTNodeResult Execute()
    {
        return BTNodeResult.InProgress;
    }

    public BTNodeResult UpdateChild()
    {
        return child.UpdateNode();
    }

    public void AddChild(BTNode childToAdd)
    {
        child = childToAdd;
    }

    public List<BTNode> GetChildren()
    {
        if(child == null)
        {
            return new List<BTNode>();  
        }
        return new List<BTNode>() { child };
    }

    public void RemoveChild(BTNode childToRemove)
    {
        if(child = childToRemove)
        {
            child = null;
        }
    }

    public void SetChildren(List<BTNode> newChildren)
    {
        if(newChildren.Count != 0)
        {
            child = newChildren[0];
        }
        else
            child = null;
    }

    public void SortChildren()
    {
        
    }

    public override BTNode CloneNode()
    {
        Decorator selfClone = Instantiate(this);
        selfClone.child = child.CloneNode();
        return selfClone;
    }

}
