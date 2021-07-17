using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum EPrefabGenerationType
{
    PlaceHere,
    PlaceBySpecificPosition
}

//Ŀ���� ������ ����� ���� MonoBeaviour�� ��ӹ޾� ���ӿ�����Ʈ�� �߰��ؼ� ���
public class PrefabGenerator : MonoBehaviour
{
    public EPrefabGenerationType genType;
    public Vector3 pos;
    public Vector3 rot;
    public GameObject prefab;
    public bool hasParent = false;
    public GameObject parent;
    public string prefabName;
}
