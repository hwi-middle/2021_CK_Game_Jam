using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameObjectsManager : MonoBehaviour
{
    [SerializeField] private GameObject ingameObjects;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(ingameObjects);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate()
    {
        ingameObjects.SetActive(true);
    }

    public void Deactivate()
    {
        ingameObjects.SetActive(false);
    }
}
