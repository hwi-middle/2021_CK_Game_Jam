using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private GameObject[] sections;
    /* 
     * 각 카테고리별 요소들을 자식으로 갖는 오브젝트들
     * 0: 사운드
     * 1: 접근성
     * 2: 조작
     */

    //사운드 설정
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SESlider;
    [SerializeField] private Slider FootstepSoundSlider;
    [SerializeField] private Image[] muteIcons; //각 섹션별 음소거 아이콘

    //접근성 설정
    [SerializeField] private Toggle VFXToggle;
    [SerializeField] private Slider ContrastSlider;
    [SerializeField] private Slider SaturationSlider;
    [SerializeField] private Slider BlueLightFilterSlider;

    [SerializeField] private Volume volume;
    private LimitlessGlitch6 glitch;
    private ColorAdjustments colorAdjustments;
    private WhiteBalance whiteBalance;

    //조작 설정
    [SerializeField] private Slider xSensitivitySlider;
    [SerializeField] private Slider ySensitivitySlider;

    //일반 설정
    [SerializeField] private Toggle keyPanelToggle;

    // Start is called before the first frame update
    void Start()
    {
        //사운드 설정
        BGMSlider.value = PlayerPrefs.GetFloat("BGMValue", 1f);
        SESlider.value = PlayerPrefs.GetFloat("SEValue", 1f);
        FootstepSoundSlider.value = PlayerPrefs.GetFloat("FootstepSoundValue", 1f);


        List<Slider> soundSliders = new List<Slider> { BGMSlider, SESlider, FootstepSoundSlider };

        for (int i = 0; i < 3; i++)
        {
            if (soundSliders[i].value <= 0)
            {
                muteIcons[i].enabled = true;
            }
            else
            {
                muteIcons[i].enabled = false;
            }
        }

        //접근성 설정
        volume.profile.TryGet(out glitch);
        volume.profile.TryGet(out colorAdjustments);
        volume.profile.TryGet(out whiteBalance);

        if (PlayerPrefs.GetInt("DisableVFX", 0) == 1)
        {
            VFXToggle.isOn = true;
        }
        else
        {
            VFXToggle.isOn = false;
        }

        ContrastSlider.value = PlayerPrefs.GetFloat("ContrastValue", 0f);
        SaturationSlider.value = PlayerPrefs.GetFloat("SaturationValue", 0f);
        BlueLightFilterSlider.value = PlayerPrefs.GetFloat("BlueLightFilterValue", 0f);

        ////조작 설정
        //xSensitivitySlider.value = PlayerPrefs.GetFloat("XSensitivityValue", 2f);
        //ySensitivitySlider.value = PlayerPrefs.GetFloat("YSensitivityValue", 2f);

        ////일반 설정
        //if (PlayerPrefs.GetInt("DisableKeyPanel", 0) == 1)
        //{
        //    keyPanelToggle.isOn = true;
        //}
        //else
        //{
        //    keyPanelToggle.isOn = false;
        //}
    }

    public void ChangeSection(string name)
    {
        foreach (var g in sections)
        {
            g.SetActive(false);
        }

        int targetIdx = -1;
        switch (name)
        {
            case "Sound":
                targetIdx = 0;
                break;
            case "Accessibility":
                targetIdx = 1;
                break;
            case "Input":
                targetIdx = 3;
                break;
            case "General":
                targetIdx = 4;
                break;
            default:
                Debug.Assert(false);
                break;
        }

        sections[targetIdx].SetActive(true);
    }

    public void ApplyBGMValue()
    {
        float val = BGMSlider.value;
        PlayerPrefs.SetFloat("BGMValue", val);
        if (val <= 0)
        {
            muteIcons[0].enabled = true;
        }
        else
        {
            muteIcons[0].enabled = false;
        }
    }

    public void ApplySEValue()
    {
        float val = SESlider.value;
        PlayerPrefs.SetFloat("SEValue", val);
        if (val <= 0)
        {
            muteIcons[1].enabled = true;
        }
        else
        {
            muteIcons[1].enabled = false;
        }
    }

    public void ApplyFootstepSoundValue()
    {
        float val = FootstepSoundSlider.value;
        PlayerPrefs.SetFloat("FootstepSoundValue", val);
        if (val <= 0)
        {
            muteIcons[2].enabled = true;
        }
        else
        {
            muteIcons[2].enabled = false;
        }
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
