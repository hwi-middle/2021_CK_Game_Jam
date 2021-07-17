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
        _target.prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", _target.prefab, typeof(GameObject), true);
        _target.hasParent = EditorGUILayout.Toggle(new GUIContent("Has Parent"), _target.hasParent);

        if (_target.hasParent)
        {
            _target.parent = (GameObject)EditorGUILayout.ObjectField("Parent", _target.parent, typeof(GameObject), true);
        }

        _target.prefabName = EditorGUILayout.TextField("Name", _target.prefabName);

        _target.genType = (EPrefabGenerationType)EditorGUILayout.EnumPopup("Type", _target.genType);
        switch (_target.genType)
        {
            case EPrefabGenerationType.PlaceBySpecificPosition:
                _target.pos = EditorGUILayout.Vector3Field("Position", _target.pos);
                break;
            case EPrefabGenerationType.PlaceHere:
                break;
        }

        _target.rot = EditorGUILayout.Vector3Field("Rotation", _target.rot);

        if (GUILayout.Button("Generate", GUILayout.Width(100)))
        {
            if (_target.prefabName == "")
            {
                Debug.LogError("Please Input the Name.");
            }
            else if (_target.parent == null)
            {
                Debug.LogError("Parent Object is Null");
            }
            else
            {
                Undo.RegisterCreatedObjectUndo(GeneratePrefab(), "Create " + name);
            }
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    public GameObject GeneratePrefab()
    {
        GameObject obj;

        if (_target.genType == EPrefabGenerationType.PlaceBySpecificPosition)
        {
            obj = PrefabUtility.InstantiatePrefab(_target.prefab) as GameObject;
            obj.transform.position = _target.pos;
            obj.transform.rotation = Quaternion.Euler(_target.rot);
        }
        else
        {
            obj = PrefabUtility.InstantiatePrefab(_target.prefab) as GameObject;
            obj.transform.position = _target.transform.position;
            obj.transform.rotation = Quaternion.Euler(_target.rot);
        }

        obj.name = _target.prefabName;
        if (_target.hasParent)
        {
            obj.transform.parent = _target.parent.transform;
        }
        return obj;
    }
}
