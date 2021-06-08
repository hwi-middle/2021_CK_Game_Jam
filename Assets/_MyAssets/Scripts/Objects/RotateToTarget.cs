using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToTarget : MonoBehaviour
{
    public GameObject target;
    private Quaternion rotation;

    void Start()
    {

    }

    void LateUpdate()
    {
        Vector3 direction = 2 * (transform.position - target.transform.position);
        rotation = Quaternion.LookRotation(direction);

        rotation.x = 0;
        rotation.z = 0;

        transform.localRotation = rotation;
    }
}
