using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public bool isAuto;
    public float delay;
    public string targetSceneName;

    // Start is called before the first frame update
    void Start()
    {
        if(isAuto)
        {
            Invoke("LoadScene", delay);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadScene()
    {
        SceneManager.LoadScene(targetSceneName);
    }

    public void LoadSceneWithTargetSceneName(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
