using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum EPrefabGenerationType
{
    PlaceHere,
    PlaceBySpecificPosition
}

//커스텀 에디터 사용을 위해 MonoBeaviour를 상속받아 게임오브젝트에 추가해서 사용
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
