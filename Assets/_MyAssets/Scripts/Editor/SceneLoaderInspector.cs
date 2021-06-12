using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SceneLoader))]

public class SceneLoaderInspector : Editor
{
    SceneLoader _target;

    private void OnEnable()
    {
        _target = target as SceneLoader;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        _target.isAuto = EditorGUILayout.Toggle(new GUIContent("Auto Start"), _target.isAuto);
        if(_target.isAuto)
        {
            _target.delay = EditorGUILayout.FloatField("Delay", _target.delay);
            _target.targetSceneName = EditorGUILayout.TextField("Target Scene Name", _target.targetSceneName);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
