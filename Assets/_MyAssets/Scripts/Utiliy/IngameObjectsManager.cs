using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameObjectsManager : MonoBehaviour
{
    [SerializeField] private GameObject ingameObjects;
    [SerializeField] private ColorVolumeModifier colorVolumeModifier;

    // Start is called before the first frame update
    void Start()
    {
        //이 스크립트는 DestroyIfAlreadyLoaded의 기능을 이미 가지고 있음
        if (PlayerPrefs.GetInt("IsIngameObjectsManagerLoaded", 0) == 1)
        {
            PlayerPrefs.SetInt("IsIngameObjectsManagerLoaded", 0);
            Destroy(gameObject);
        }

        PlayerPrefs.SetInt("IsIngameObjectsManagerLoaded", 1);

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
