using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OptionsController : MonoBehaviour
{
    [SerializeField] private Toggle VFXToggle;
    [SerializeField] private Slider ContrastSlider;
    [SerializeField] private Slider SaturationSlider;
    [SerializeField] private Slider BlueLightFilterSlider;

    [SerializeField] private Volume volume;
    private LimitlessGlitch6 glitch;
    private ColorAdjustments colorAdjustments;
    private WhiteBalance whiteBalance;



    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGet(out glitch);
        volume.profile.TryGet(out colorAdjustments);
        volume.profile.TryGet(out whiteBalance);

        if(PlayerPrefs.GetInt("DisableVFX")==1)
        {
            VFXToggle.isOn = true;
        }
        else
        {
            VFXToggle.isOn = false;
        }

        ContrastSlider.value = PlayerPrefs.GetFloat("ContrastValue");
        SaturationSlider.value = PlayerPrefs.GetFloat("SaturationValue");
        BlueLightFilterSlider.value = PlayerPrefs.GetFloat("BlueLightFilterValue");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ApplyVFXValue()
    {
        if (VFXToggle.isOn)
        {
            PlayerPrefs.SetInt("DisableVFX", 1);
            glitch.enable.value = false;
        }
        else
        {
            PlayerPrefs.SetInt("DisableVFX", 0);
            glitch.enable.value = true;
        }
    }

    public void ApplyContrastValue()
    {
        float val = ContrastSlider.value;
        PlayerPrefs.SetFloat("ContrastValue", val);
        colorAdjustments.contrast.value = val;
    }

    public void ApplySaturationValue()
    {
        float val = SaturationSlider.value;
        PlayerPrefs.SetFloat("SaturationValue", val);
        colorAdjustments.saturation.value = val;
    }

    public void ApplyBlueLightFilterValue()
    {
        float val = BlueLightFilterSlider.value;
        PlayerPrefs.SetFloat("BlueLightFilterValue", val);
        whiteBalance.temperature.value = val;
    }

    public void ResetAccessibilityValues()
    {
        VFXToggle.isOn = false;
        glitch.enable.value = true;

        ContrastSlider.value = 0;
        PlayerPrefs.SetFloat("ContrastValue", 0);
        colorAdjustments.contrast.value = 0;

        SaturationSlider.value = 0;
        PlayerPrefs.SetFloat("SaturationValue", 0);
        colorAdjustments.saturation.value = 0;

        BlueLightFilterSlider.value = 0;
        PlayerPrefs.SetFloat("BlueLightFilterValue", 0);
        whiteBalance.temperature.value = 0;
    }
}
