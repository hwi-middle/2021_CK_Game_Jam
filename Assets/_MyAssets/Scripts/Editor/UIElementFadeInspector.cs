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
        _target = (UIElementFade)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        _target.fadeType = (EFadeType)EditorGUILayout.EnumPopup("Fade Type", _target.fadeType);
        _target.autoPlay = EditorGUILayout.Toggle(new GUIContent("Auto Start"), _target.autoPlay);
        if (_target.autoPlay)
        {
            _target.delay = EditorGUILayout.FloatField(new GUIContent("Delay"), _target.delay);
        }
        _target.duration = EditorGUILayout.FloatField(new GUIContent("Duration"), _target.duration);
    }
}
