using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabGenerator : MonoBehaviour
{
    public GameObject prefab;
    public string prefabName;

    void Start()
    {

    }

    public GameObject GeneratePrefab()
    {
        var obj = Instantiate(prefab, transform.position, Quaternion.identity);
        obj.name = prefabName;
        return obj;
    }
}
