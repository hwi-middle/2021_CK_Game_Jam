using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoSceneLoader : MonoBehaviour
{
    public float time;
    public string targetSceneName;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("LoadTargetScene", time);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadTargetScene()
    {
        SceneManager.LoadScene(targetSceneName);
    }
}
