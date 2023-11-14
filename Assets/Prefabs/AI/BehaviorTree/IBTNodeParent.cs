using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//interface are like abstract classes that has all abstract functions and no data member
public interface IBTNodeParent
{
    void SetChildren(List<BTNode> newChildren);
    List<BTNode> GetChildren();
    void RemoveChild(BTNode childToRemove);
    void AddChild(BTNode childToAdd);
    void SortChildren();
}
