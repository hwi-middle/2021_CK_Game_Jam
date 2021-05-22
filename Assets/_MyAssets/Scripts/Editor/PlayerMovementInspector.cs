using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(PlayerMovement))]

public class PlayerMovementInspector : Editor
{

    PlayerMovement _target;
    private ReorderableList list;

    bool showPlayerMovement = true;
    bool showFootstep = true;
    bool showDangerZone = false;

    private void OnEnable()
    {
        _target = target as PlayerMovement;
        list = new ReorderableList(serializedObject,
         serializedObject.FindProperty("audioDatas"),
         true, true, true, true);

        // Element 가 그려질 때 Callback
        list.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                // 현재 그려질 요소
                SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2; // 위쪽 패딩
                EditorGUI.PropertyField(
                    new Rect(rect.x + 5, rect.y, 50, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("isActivated"), new GUIContent("Clip " + index.ToString()));

                EditorGUI.PropertyField(
                    new Rect(rect.x + 50, rect.y, rect.width - 50, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("clip"), GUIContent.none
                    );


            };

        list.drawHeaderCallback = rect =>
        {
            EditorGUI.LabelField(rect, "Audio Clips", EditorStyles.boldLabel);
        };

        list.onRemoveCallback = (ReorderableList l) =>
        {
            string warningText = "Are you sure you want to remove this clip?\n" +
            "You cannot undo this action!\n\n" +
            "Clip index: " + l.index.ToString() + "\n" +
            "Please note that this number starts at zero.\n";

            if (EditorUtility.DisplayDialog("Warning!", warningText, "Yes", "No"))
            {
                ReorderableList.defaultBehaviours.DoRemoveButton(l);
            }
        };
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
            _target.frequency = EditorGUILayout.Slider(new GUIContent("Frequency"), _target.frequency, 0, 5);
            EditorGUILayout.Space();
            serializedObject.Update();
            list.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
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
