using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameObjectsManager : MonoBehaviour
{
    public static bool isLoaded = false;
    [SerializeField] private GameObject ingameObjects;
    [SerializeField] private ColorVolumeModifier colorVolumeModifier;

    // Start is called before the first frame update
    void Start()
    {
        if (isLoaded)
        {
            Destroy(gameObject);
        }

        isLoaded = true;

        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(ingameObjects);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("ShouldReActivateIngameObjects", 0) == 1)
        {
            colorVolumeModifier.UpdateValue();
            Activate();
            PlayerPrefs.SetInt("ShouldReActivateIngameObjects", 0);
        }
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
