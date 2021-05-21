using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UIElementFade))]

public class UIElementInspctor : Editor
{

    UIElementFade _target;

    private void OnEnable()
    {
        _target = target as UIElementFade;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        _target.fadeType = (EFadeType)EditorGUILayout.EnumPopup("Fade Type", _target.fadeType);
        _target.autoPlay = EditorGUILayout.Toggle(new GUIContent("Auto Start"), _target.autoPlay);
        if (_target.autoPlay)
        {
            _target.delay = EditorGUILayout.Slider(new GUIContent("Delay"), _target.delay, 0, 10);
        }
        _target.duration = EditorGUILayout.Slider(new GUIContent("Duration"), _target.duration, 0, 10);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
