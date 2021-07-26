using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayOnMapIfDetected : MonoBehaviour
{
    private GameObject indicator;

    // Start is called before the first frame update
    void Start()
    {
        indicator = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.layer == (int)EEnemyTypeOnMap.Detected)
        {
            indicator.SetActive(true);
        }
        else
        {
            indicator.SetActive(false);
        }
    }
}
