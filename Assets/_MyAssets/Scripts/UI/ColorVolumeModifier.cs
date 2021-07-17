using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColorVolumeModifier : MonoBehaviour
{
    [SerializeField] private Volume volume;

    private ColorAdjustments colorAdjustments;
    private WhiteBalance whiteBalance;

    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGet(out colorAdjustments);
        volume.profile.TryGet(out whiteBalance);

        UpdateValue();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //인게임 설정에서도 값을 변경할 수 있도록 함수화
    public void UpdateValue()
    {
        colorAdjustments.contrast.value = PlayerPrefs.GetFloat("ContrastValue");
        colorAdjustments.saturation.value = PlayerPrefs.GetFloat("SaturationValue");
        whiteBalance.temperature.value = PlayerPrefs.GetFloat("BlueLightFilterValue");
    }
}
