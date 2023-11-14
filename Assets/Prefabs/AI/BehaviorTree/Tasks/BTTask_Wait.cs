using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTTask_Wait : BTNode
{
    [SerializeField] float waitTime = 2f;
    float timeElapsed;
    protected override BTNodeResult Execute()
    {
        if(waitTime == 0)
        {
            return BTNodeResult.Success;
        }
        Debug.Log("Wait started");
        timeElapsed = 0;
        return BTNodeResult.InProgress;
    }

    protected override BTNodeResult Update()
    {
        timeElapsed += Time.deltaTime;
        if(timeElapsed >= waitTime) 
        {
            Debug.Log("Wait ended");
            return BTNodeResult.Success;             
        }
        return BTNodeResult.InProgress;
    }

    internal void SetWaitTime(int newWaitTime)
    {
        waitTime = newWaitTime;
    }
}
