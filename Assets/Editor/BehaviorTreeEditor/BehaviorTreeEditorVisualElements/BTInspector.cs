using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BTInspector : VisualElement
{
    Editor editor;
    internal void ShowInspectorGUI(BTNode node)
    {
        Clear();

        UnityEngine.Object.DestroyImmediate(editor);

        editor = Editor.CreateEditor(node);

        IMGUIContainer container = new IMGUIContainer(() => 
        {
            if (editor.target)
            {
                editor.OnInspectorGUI();
            }
        });

        Add(container);
    }

    public new class UxmlFactory : UxmlFactory<BTInspector, UxmlTraits> { };


}
