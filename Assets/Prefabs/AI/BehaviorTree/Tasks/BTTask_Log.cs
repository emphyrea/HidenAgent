using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTTask_Log : BTNode
{
    [SerializeField] string message;
    public void SetMessage(string message)
    {
        this.message = message;
    }
    protected override BTNodeResult Execute()
    {
        Debug.Log(message);
        return BTNodeResult.Success;
    }
}
