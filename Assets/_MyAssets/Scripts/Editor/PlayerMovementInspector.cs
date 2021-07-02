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
    bool showMouseLook = true;
    bool showDangerZone = false;

    private void OnEnable()
    {
        _target = target as PlayerMovement;

        //Audio Data 관련 Reorderable List 설정

        //초기화
        list = new ReorderableList(serializedObject,
         serializedObject.FindProperty("audioDatas"),
         true, true, true, true);

        //인스펙터에 그리기
        list.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;    //보기좋게 상단에 Padding
                var origin = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 45;   //레이블 폭 줄이기
                EditorGUI.PropertyField(
                    new Rect(rect.x + 5, rect.y, 10, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("isActivated"), new GUIContent("Clip " + index.ToString()));

                EditorGUI.PropertyField(
                    new Rect(rect.x + 80, rect.y, rect.width - 80, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("clip"), GUIContent.none
                    );
                EditorGUIUtility.labelWidth = origin;
            };

        //헤더
        list.drawHeaderCallback = rect =>
        {
            EditorGUI.LabelField(rect, "Audio Clips", EditorStyles.boldLabel);
        };

        //요소 삭제 시 경고창 출력
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
                _target.alternativeSpeedScale = EditorGUILayout.FloatField("Alternative Speed Scale", _target.alternativeSpeedScale);
                EditorGUI.indentLevel--;

                _target.hasStamina = EditorGUILayout.Toggle(new GUIContent("Has Stamina"), _target.hasStamina);
                if (_target.hasStamina)
                {
                    EditorGUI.indentLevel++;
                    _target.maxStamina = EditorGUILayout.FloatField("Max Stamina Amount", _target.maxStamina);
                    _target.staminaDecreasementAmount = EditorGUILayout.FloatField("Stamina Decreasement Amount", _target.staminaDecreasementAmount);
                    _target.staminaIncreasementAmount = EditorGUILayout.FloatField("Stamina Increasement Amount", _target.staminaIncreasementAmount);
                    _target.staminaIncresementDelay = EditorGUILayout.FloatField("Stamina Incresement Delay", _target.staminaIncresementDelay);
                    EditorGUI.indentLevel--;
                }
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

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Activate All", GUILayout.Width(100)))
            {
                for(int i = 0; i < _target.audioDatas.Count;i++)
                {
                    AudioData temp = _target.audioDatas[i];
                    temp.isActivated = true;
                    _target.audioDatas[i] = temp;
                }
            }
            if (GUILayout.Button("Deactivate All", GUILayout.Width(100)))
            {
                for (int i = 0; i < _target.audioDatas.Count; i++)
                {
                    AudioData temp = _target.audioDatas[i];
                    temp.isActivated = false;
                    _target.audioDatas[i] = temp;
                }
            }
            EditorGUILayout.EndHorizontal();

        }

        showMouseLook = EditorGUI.Foldout(EditorGUILayout.GetControlRect(), showMouseLook, "Mouse Look", true);
        if (showMouseLook)
        {
            _target.sensitivityX = EditorGUILayout.FloatField(new GUIContent("X Sensitivity"), _target.sensitivityX);
            _target.sensitivityY = EditorGUILayout.FloatField(new GUIContent("Y Sensitivity"), _target.sensitivityY);
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
