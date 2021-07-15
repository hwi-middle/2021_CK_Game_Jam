using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum EPrefabGenerationType
{
    PlaceHere,
    PlaceBySpecificPosition
}

public class PrefabGenerator : MonoBehaviour
{
    public EPrefabGenerationType genType;
    public Vector3 pos;
    public Vector3 rot;
    public GameObject prefab;
    public bool hasParent = false;
    public GameObject parent;
    public string prefabName;

    void Start()
    {

    }

    public GameObject GeneratePrefab()
    {
        GameObject obj;

        if (genType == EPrefabGenerationType.PlaceBySpecificPosition)
        {
            obj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            obj.transform.position = pos;
            obj.transform.rotation = Quaternion.Euler(rot);
        }
        else
        {
            obj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            obj.transform.position = transform.position;
            obj.transform.rotation = Quaternion.Euler(rot);
        }

        obj.name = prefabName;
        if (hasParent)
        {
            obj.transform.parent = parent.transform;
        }
        return obj;
    }
}
