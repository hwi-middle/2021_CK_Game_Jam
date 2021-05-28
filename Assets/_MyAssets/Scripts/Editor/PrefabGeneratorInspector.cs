using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PrefabGenerator))]

public class PrefabGeneratorInspector : Editor
{
    PrefabGenerator _target;

    private void OnEnable()
    {
        _target = target as PrefabGenerator;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        _target.prefabName = EditorGUILayout.TextField("Name", _target.prefabName);

        if (GUILayout.Button("Generate", GUILayout.Width(100)))
        {
            if (_target.prefabName == "")
            {
                Debug.LogError("Please Input the Name.");
            }
            else
            {
                Undo.RegisterCreatedObjectUndo(_target.GeneratePrefab(), "Create" + name);
            }
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
