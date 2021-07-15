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
            _target.startDelay = EditorGUILayout.Slider(new GUIContent("Start Delay"), _target.startDelay, 0, 10);
        }

        switch (_target.fadeType)
        {
            case EFadeType.FadeIn:
            case EFadeType.FadeOut:
                _target.duration1 = EditorGUILayout.Slider(new GUIContent("Duration"), _target.duration1, 0, 10);
                break;
            case EFadeType.FadeOutAndFadeIn:
            case EFadeType.FadeInAndFadeOut:
                _target.duration1 = EditorGUILayout.Slider(new GUIContent("Duration 1"), _target.duration1, 0, 10);
                _target.fadeDelay = EditorGUILayout.Slider(new GUIContent("Fade Delay"), _target.fadeDelay, 0, 10);
                _target.duration2 = EditorGUILayout.Slider(new GUIContent("Duration 2"), _target.duration2, 0, 10);
                break;
            default:
                Debug.Assert(false);
                break;
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
