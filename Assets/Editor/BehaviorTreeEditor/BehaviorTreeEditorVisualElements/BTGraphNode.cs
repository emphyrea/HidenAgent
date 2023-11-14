using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BTGraphNode : Node
{
    public BTNode Node { get; private set; }
    Port inputPort;
    Port outputPort;

    public delegate void OnNodeSelected(BTNode node);
    public event OnNodeSelected onNodeSelected;

    public Port GetInputPort()
    {
        return inputPort;
    }

    public Port GetOutputPort()
    {
        return outputPort;
    }


    public BTGraphNode(BTNode node)
    {
        Node = node;
        title = node.GetType().Name;
        style.left = node.GetGraphPosition().x;
        style.top = node.GetGraphPosition().y;

        CreatePorts();
        viewDataKey = node.GetGUID();
        node.onNodeStateChanged += NodeStateChanged;
    }

    private void NodeStateChanged(BTNodeResult newState)
    {
        switch (newState)
        {
            case BTNodeResult.Success:
                style.backgroundColor = Color.gray;
                break;
            case BTNodeResult.InProgress:
                style.backgroundColor = Color.green;
                break;
            case BTNodeResult.Failure:
                style.backgroundColor = Color.red;
                break;
            default:
                break;
        }
    }

    private void CreatePorts()
    {
        switch (Node.GetInputPortType())
        {
            case BTNodePortType.Single:
                inputPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(BTNode));
                inputPort.portName = "parent";
                break;
            default:
                break;
        }
        if(inputPort != null)
        {
            inputContainer.Add(inputPort);
        }
        switch (Node.GetOutputPortType())
        {
            case BTNodePortType.Single:
                outputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(BTNode));
                outputPort.portName = "child";
                break;
            case BTNodePortType.Multi:
                outputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(BTNode));
                outputPort.portName = "children";
                break;
            default:
                break;
        }
        if (outputPort != null)
        {
            outputContainer.Add(outputPort);
        }
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        Undo.RecordObject(Node, "Move Behavior Tree Node");
        Node.SetGraphPosition(new Vector2(newPos.xMin, newPos.yMin));
        EditorUtility.SetDirty(Node);
    }

    public override void OnSelected()
    {
        base.OnSelected();
        onNodeSelected?.Invoke(Node);
    }
}
