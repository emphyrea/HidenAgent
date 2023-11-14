using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTTask_AlwaysFail : BTNode
{
    protected override BTNodeResult Execute()
    {
        Debug.Log("AlwaysFailed");
        return BTNodeResult.Failure;
    }
}
