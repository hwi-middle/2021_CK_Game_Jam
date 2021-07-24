using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("IsFromIngame", 0);
        PlayerPrefs.SetInt("ShouldReActivateIngameObjects", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
