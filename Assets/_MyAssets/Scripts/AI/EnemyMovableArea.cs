using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovableArea : MonoBehaviour
{
    [SerializeField] private Enemy target;

    void Start()
    {
        if(target.Type != EEnemyType.Sub)
        {
            Debug.LogError("Target enemy is not sub enemy");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Sub Enemy")
        {
            Debug.LogError("제한구역 이탈");
            target.StopAndReturnToOrigin();
        }
    }
}
