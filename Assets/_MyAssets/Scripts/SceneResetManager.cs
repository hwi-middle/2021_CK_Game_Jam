using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneResetManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ClaerAllObjects()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("GameController");

        foreach (var o in objects)
        {
            Destroy(o);
        }

        Time.timeScale = 1f;
        IngameObjectsManager.isLoaded = false;
        DestroyIfAlreadyLoaded.isLoaded = false;
    }

    public void ClearAllObjectsAndLoadScene(string target)
    {
        ClaerAllObjects();

        SceneManager.LoadScene(target);
    }
}
