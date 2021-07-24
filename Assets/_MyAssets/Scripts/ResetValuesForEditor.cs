using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetValuesForEditor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("IsIngameObjectsManagerLoaded", 0);
        PlayerPrefs.SetInt("IsIngameObjectsLoaded", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
