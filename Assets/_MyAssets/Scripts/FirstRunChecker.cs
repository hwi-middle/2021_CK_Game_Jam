using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstRunChecker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int firstRun = PlayerPrefs.GetInt("FirstRun", 1);

        if (firstRun == 1)
        {
            PlayerPrefs.SetInt("FirstRun", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
