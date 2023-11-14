using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBTTaskInterface
{
    public void RotateTowards(Vector3 direction);

    public void RotateTowards(GameObject target);

    public void AttackTarget(GameObject target);
}
