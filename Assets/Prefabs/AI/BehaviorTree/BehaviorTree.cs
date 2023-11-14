using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(menuName = "BehaviorTree/BehaviorTree")]
public class BehaviorTree : ScriptableObject
{
    public
    BTNodeRoot rootNode;

    [SerializeField] List<BTNode> nodes;

    [SerializeField] Blackboard blackboard;

    BTNode currentNode;


    public Blackboard GetBlackboard() {return blackboard;}

    public List<BTNode> GetNodes() { return nodes; }

    public void PreConstruct()
    {
        if(!rootNode)
        {
            rootNode = CreateNode(typeof(BTNodeRoot)) as BTNodeRoot;
            SaveTree();
        }
        Construct(rootNode);
    }
    protected virtual void Construct(BTNodeRoot root)
    {
        
    }

    public void AbortIfCurrentIsLower(int priority)
    {
        if (currentNode.GetPriority() > priority)
        {
            rootNode.End();
        }
    }

    public void Start()
    {
        SortTree();
        foreach(BTNode node in nodes)
        {
            node.onBecomeActive += CurrentNodeChanged;
        }
    }

    private void CurrentNodeChanged(BTNode node)
    {
        currentNode = node;
    }

    public void Update()
    {
        rootNode.UpdateNode();
    }

    public BTNode CreateNode(System.Type nodeType)
    {
        BTNode newNode = ScriptableObject.CreateInstance(nodeType) as BTNode;
        nodes.Add(newNode);

        newNode.name = nodeType.Name;
        newNode.Init(this);
        AssetDatabase.AddObjectToAsset(newNode, this);

        SaveTree();

        return newNode;
    }

    public void SaveTree()
    {
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssetIfDirty(this);
    }

    public void RemoveNode(BTNode node)
    {
        nodes.Remove(node);
        //AssetDatabase.RemoveObjectFromAsset(node);
        SaveTree();
    }

    public void SortTree()
    {
        foreach(BTNode node in nodes)
        {
            IBTNodeParent parent = node as IBTNodeParent;
            if(parent != null)
            {
                parent.SortChildren();
            }
        }
        int priorityCounter = 0;
        Traverse(rootNode, (BTNode n) => {n.SortPriority(ref priorityCounter); });
    }

    internal BehaviorTree CloneTree()
    {
        BehaviorTree clone = Instantiate(this);
        clone.rootNode = rootNode.CloneNode() as BTNodeRoot;

        clone.nodes = new List<BTNode>();
        clone.blackboard = Instantiate(blackboard);

        Traverse(clone.rootNode, (BTNode node) => 
        { 
            clone.nodes.Add(node); 
            node.Init(clone);
        });

        return clone;
    }

    public void Traverse(BTNode node, System.Action<BTNode> visitor)
    {
        visitor(node);
        IBTNodeParent nodeAsParent = node as IBTNodeParent;
        if(nodeAsParent != null)
        {
            foreach(BTNode child in nodeAsParent.GetChildren())
            {
                Traverse(child, visitor);
            }
        }
    }

    internal void Stop()
    {
        rootNode.End();
    }
}
