using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            obj = Instantiate(prefab, pos, Quaternion.Euler(rot));

        }
        else
        {
            obj = Instantiate(prefab, transform.position, Quaternion.Euler(rot));
        }

        obj.name = prefabName;
        if (hasParent)
        {
            obj.transform.parent = parent.transform;
        }
        return obj;
    }
}
