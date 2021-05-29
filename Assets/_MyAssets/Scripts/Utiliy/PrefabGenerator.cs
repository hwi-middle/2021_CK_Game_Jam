using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabGenerator : MonoBehaviour
{
    public GameObject prefab;
    public bool hasParent = false;
    public GameObject parent;
    public string prefabName;

    void Start()
    {

    }

    public GameObject GeneratePrefab()
    {
        var obj = Instantiate(prefab, transform.position, Quaternion.identity);
        obj.name = prefabName;
        if(hasParent)
        {
            obj.transform.parent = parent.transform;
        }
        return obj;
    }
}
