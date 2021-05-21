using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerMovement))]

public class PlayerMovementInspector : Editor
{

    PlayerMovement _target;

    bool showPlayerMovement = true;
    bool showFootstep = true;
    bool showAudioClip = false;
    bool showDangerZone = false;

    private void OnEnable()
    {
        _target = target as PlayerMovement;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        showPlayerMovement = EditorGUI.Foldout(EditorGUILayout.GetControlRect(), showPlayerMovement, "Player Movement", true);
        if (showPlayerMovement)
        {
            EditorGUI.indentLevel++;
            _target.gravityScale = EditorGUILayout.FloatField("Gravity Scale", _target.gravityScale);
            _target.speed = EditorGUILayout.FloatField("Speed", _target.speed);
            _target.hasAlternativeSpeed = EditorGUILayout.Toggle(new GUIContent("Has Alternative Speed"), _target.hasAlternativeSpeed);
            if (_target.hasAlternativeSpeed)
            {
                EditorGUI.indentLevel++;
                _target.alternativeSpeedFactor = EditorGUILayout.FloatField("Alternative Speed Scale", _target.alternativeSpeedFactor);
                EditorGUI.indentLevel--;
            }
            _target.canJump = EditorGUILayout.Toggle(new GUIContent("Can Jump"), _target.canJump);
            if (_target.canJump)
            {
                EditorGUI.indentLevel++;
                _target.jumpHeight = EditorGUILayout.Slider(new GUIContent("Jump Height"), _target.jumpHeight, 0, 30);
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
        }

        showFootstep = EditorGUI.Foldout(EditorGUILayout.GetControlRect(), showFootstep, "Footstep Sound", true);
        if (showFootstep)
        {
            EditorGUI.indentLevel++;
            //EditorGUILayout.LabelField("Audio Clips", EditorStyles.boldLabel);
            showAudioClip = EditorGUI.Foldout(EditorGUILayout.GetControlRect(), showAudioClip, "Audio Clips", true);
            if (showAudioClip)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < _target.audioClips.Length; i++)
                {
                    string fieldName = "Clip " + i;
                    _target.audioClips[i] = (AudioClip)EditorGUILayout.ObjectField(fieldName, _target.audioClips[i], typeof(AudioClip), false);
                }
                EditorGUI.indentLevel--;
            }
            _target.frequency = EditorGUILayout.Slider(new GUIContent("Frequency"), _target.frequency, 0, 5);
            EditorGUI.indentLevel--;
        }
        showDangerZone = EditorGUI.Foldout(EditorGUILayout.GetControlRect(), showDangerZone, "Danger Zone ", true);
        if (showDangerZone)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.HelpBox("If you are not a programmer, DO NOT MODIFY THE VALUES BELOW.", MessageType.Error);
            _target.groundCheck = (Transform)EditorGUILayout.ObjectField("Ground Cheack Object", _target.groundCheck, typeof(Transform), true);
            _target.groundDistance = EditorGUILayout.FloatField("Ground Distance", _target.groundDistance);
            EditorGUI.indentLevel--;
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
