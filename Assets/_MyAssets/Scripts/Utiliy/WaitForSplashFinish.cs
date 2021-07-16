using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class WaitForSplashFinish : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        while (!SplashScreen.isFinished)
        {
            yield return null;
        }

        SceneManager.LoadScene("Intro1");
    }

    // Update is called once per frame
    void Update()
    {
        if(SplashScreen.isFinished)
        {

        }
    }
}
