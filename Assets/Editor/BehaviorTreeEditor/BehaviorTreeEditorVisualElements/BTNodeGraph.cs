using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class BTNodeGraph : GraphView
{
    public new class UxmlFactory : UxmlFactory<BTNodeGraph, UxmlTraits> { }

    BehaviorTree tree;

    public event BTGraphNode.OnNodeSelected onNodeSelected;

    public BTNodeGraph()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        StyleSheet stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviorTreeEditor/BehaviorTreeEditor.uss");
        styleSheets.Add(stylesheet);

        graphViewChanged += GraphChange;

        Undo.undoRedoPerformed += RefreshGraph;
    }

    private void RefreshGraph()
    {
        if(tree)
        {
            PopulateTree(tree);
        }
    }

    private GraphViewChange GraphChange(GraphViewChange graphViewChange)
    {
        if(graphViewChange.edgesToCreate != null)
        {
            HandleEdgeCreation(graphViewChange.edgesToCreate);
        }

        if(graphViewChange.elementsToRemove != null)
        {
            HandleElementRemoval(graphViewChange.elementsToRemove);
        }

        if(graphViewChange.movedElements != null)
        {
            tree.SortTree();
        }

        return graphViewChange;
    }

    private void HandleElementRemoval(List<GraphElement> elementsToRemove)
    { 
        //handles node removal
        GraphElement elementToKeep = null;

        foreach(GraphElement element in elementsToRemove)
        {
            BTGraphNode graphNode = element as BTGraphNode;
            if(graphNode != null)
            {
                if (graphNode.Node.GetType() == typeof(BTNodeRoot))
                {
                    elementToKeep = element;
                }
                else
                {
                    Undo.RecordObject(tree, "Delete Behavior Tree Node");
                    tree.RemoveNode(graphNode.Node);
                    Undo.DestroyObjectImmediate(graphNode.Node);
                    tree.SaveTree();
                }
            }

            //handle edge removal

            Edge edge = element as Edge;

            if(edge != null)
            {
                BTGraphNode inputGraphNode = edge.output.node as BTGraphNode;
                BTGraphNode outputGraphNode = edge.input.node as BTGraphNode;

                IBTNodeParent parent = inputGraphNode.Node as IBTNodeParent;
                if(parent != null)
                {
                    Undo.RecordObject(inputGraphNode.Node, "Delete Behavior Tree Connection");
                    parent.RemoveChild(outputGraphNode.Node);
                    EditorUtility.SetDirty(inputGraphNode.Node);
                }
            }
        }

        //prevent root node from being removed

        if(elementToKeep != null)
        {
            elementsToRemove.Remove(elementToKeep);
        }
    }

    private void HandleEdgeCreation(List<Edge> edgesToCreate)
    {
        foreach(Edge edge in edgesToCreate)
        {
            BTGraphNode inputGraphNode = edge.output.node as BTGraphNode;
            BTGraphNode outputGraphNode = edge.input.node as BTGraphNode;

            IBTNodeParent parent = inputGraphNode.Node as IBTNodeParent;
            if(parent != null)
            {
                parent.AddChild(outputGraphNode.Node);
            }
        }
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        base.BuildContextualMenu(evt);

        var allNodeTypes = TypeCache.GetTypesDerivedFrom<BTNode>();

        foreach (System.Type nodeType in allNodeTypes )
        {
            if(nodeType.IsAbstract)
            {
                continue;
            }
            if(nodeType == typeof(BTNodeRoot))
            {
                continue;
            }

            evt.menu.AppendAction(nodeType.Name, (arg) => CreateNode(nodeType));

        }
    }

    private void CreateNode(Type nodeType)
    {
        Undo.RecordObject(tree, "Add Behavior Tree Node");
        BTNode newNode = tree.CreateNode(nodeType);
        Undo.RegisterCreatedObjectUndo(newNode, "Add Behavior Tree Node");
        CreateGraphNode(newNode);

    }

    private void CreateGraphNode(BTNode newNode)
    {
        BTGraphNode newGraphNode = new BTGraphNode(newNode);
        newGraphNode.onNodeSelected += onNodeSelected;
        AddElement(newGraphNode);
    }

    internal void PopulateTree(BehaviorTree selectedAsTree)
    {
        SaveTree();

        graphViewChanged -= GraphChange;
        DeleteElements(graphElements);
        graphViewChanged += GraphChange;

        Debug.Log($"Populating Tree: {selectedAsTree.name}");
        tree = selectedAsTree;
        tree.PreConstruct();
        foreach(BTNode node in tree.GetNodes())
        {
            CreateGraphNode(node);
        }
        //creating edges
        foreach(BTNode node in tree.GetNodes())
        {
            IBTNodeParent nodeAsParent = node as IBTNodeParent;
            if(nodeAsParent != null)
            {
                BTGraphNode graphNode = GetNodeByGuid(node.GetGUID()) as BTGraphNode;

                foreach(BTNode child in nodeAsParent.GetChildren())
                {
                    BTGraphNode childGraphNode = GetNodeByGuid(child.GetGUID()) as BTGraphNode;

                    Edge newEdge = graphNode.GetOutputPort().ConnectTo(childGraphNode.GetInputPort());
                    AddElement(newEdge);
                }
            }
        }
    }

    internal void SaveTree()
    {
        if(tree)
        {
            tree.SaveTree();
        }
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> compatiblePorts = new List<Port>();
        foreach(Port port in ports)
        {
            if(startPort == port)
            {
                continue;
            }
            if (port.node == startPort.node)
            {
                continue;
            }
            if(port.direction == startPort.direction)
            {
                continue;
            }

            BTGraphNode graphNode = port.node as BTGraphNode;
            BTGraphNode startPortGraphNode = startPort.node as BTGraphNode;

            BTNode node = graphNode.Node;
            BTNode startNode = startPortGraphNode.Node;
            if(startPort.direction == Direction.Output)
            {
                if(node.Contains(startNode))
                {
                    continue;
                }
            }

            compatiblePorts.Add(port);
        }
        return compatiblePorts;
    }
}
