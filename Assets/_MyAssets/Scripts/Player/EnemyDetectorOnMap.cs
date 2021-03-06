using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectorOnMap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Main Enemy" || other.tag == "Sub Enemy")
        {
            other.gameObject.layer = (int)EEnemyTypeOnMap.Detected;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Main Enemy" || other.tag == "Sub Enemy")
        {
            other.gameObject.layer = (int)EEnemyTypeOnMap.Normal;
        }
    }
}
