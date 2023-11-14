using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequencer : Compositor
{
    protected override BTNodeResult Update()
    {
        BTNodeResult result = UpdateCurrent();

        if (result == BTNodeResult.Failure)
        {
            return BTNodeResult.Failure;
        }

        if (result == BTNodeResult.Success)
        {
            if(!Next())
            {
                return BTNodeResult.Success;
            }
        }
            return BTNodeResult.InProgress;
    }
}
