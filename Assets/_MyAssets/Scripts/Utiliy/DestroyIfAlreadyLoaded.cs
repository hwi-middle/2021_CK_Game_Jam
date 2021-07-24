using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIfAlreadyLoaded : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("IsIngameObjectsLoaded", 0) == 1)
        {
            PlayerPrefs.SetInt("IsIngameObjectsLoaded", 0);
            Destroy(gameObject);
        }

        PlayerPrefs.SetInt("IsIngameObjectsLoaded", 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
