using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BTBlackboardInspector : VisualElement
{
    public new class UxmlFactory : UxmlFactory<BTBlackboardInspector, UxmlTraits> { };

    Editor editor;
    IMGUIContainer container;
    public void ShowInspectorForBlackboard(Blackboard blackboard)
    {
        Clear();

        UnityEngine.Object.DestroyImmediate(editor);
        container = null;
        if(!blackboard)
        {
            return;
        }
        editor = Editor.CreateEditor(blackboard);
        container = new IMGUIContainer(ShowInspector);
        Add(container);

    }

    private void ShowInspector()
    {
        if(editor.target)
        {
            editor.OnInspectorGUI();
        }
    }
}
