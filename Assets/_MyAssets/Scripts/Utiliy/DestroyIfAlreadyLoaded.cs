using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIfAlreadyLoaded : MonoBehaviour
{
    static bool isLoaded = false;
    // Start is called before the first frame update
    void Start()
    {
        if (isLoaded)
        {
            Destroy(gameObject);
        }
        isLoaded = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
