using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Selector : Compositor
{
    protected override BTNodeResult Update()
    {
        BTNodeResult result = UpdateCurrent();

        if (result == BTNodeResult.Failure)
        {
            if(!Next())
            {
                 return BTNodeResult.Failure;
            }
        }

        if (result == BTNodeResult.Success)
        {
            return BTNodeResult.Success;
        }
        return BTNodeResult.InProgress;
    }
}
