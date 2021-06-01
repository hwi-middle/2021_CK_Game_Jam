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

        if(genType == EPrefabGenerationType.PlaceBySpecificPosition)
        {
            obj = Instantiate(prefab, pos, Quaternion.identity);

        }
        else
        {
            obj = Instantiate(prefab, transform.position, Quaternion.identity);

        }

        obj.name = prefabName;
        if(hasParent)
        {
            obj.transform.parent = parent.transform;
        }
        return obj;
    }
}
