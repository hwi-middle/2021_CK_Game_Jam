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
    [SerializeField] private Text xSensitivityText;
    [SerializeField] private Text ySensitivityText;
    [SerializeField] private Text sensitivityPreviewText;
    [SerializeField] private Transform sensitivityPreviewCam;
    [SerializeField] private Transform sensitivityPreviewBody;

    //일반 설정
    [SerializeField] private Toggle keyInfoPanelToggle;

    //감마 설정
    [SerializeField] private Volume gammaVolume;
    private LiftGammaGain liftGammaGain;
    [SerializeField] private Text gammaAmountText;
    private float currentGamma;

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

        //조작 설정
        xSensitivitySlider.value = PlayerPrefs.GetFloat("XSensitivityValue", 2f);
        ySensitivitySlider.value = PlayerPrefs.GetFloat("YSensitivityValue", 2f);
        xSensitivityText.text = string.Format("{0:0.##}", xSensitivitySlider.value);
        ySensitivityText.text = string.Format("{0:0.##}", ySensitivitySlider.value);
        SetSensitivityPreviewText(false);

        ////일반 설정
        if (PlayerPrefs.GetInt("DisableKeyInfoPanel", 0) == 1)
        {
            keyInfoPanelToggle.isOn = true;
        }
        else
        {
            keyInfoPanelToggle.isOn = false;
        }

        //감마 설정
        currentGamma = PlayerPrefs.GetFloat("GammaValue", 0f);
        gammaAmountText.text = currentGamma.ToString("+#;-#;0");

        gammaVolume.profile.TryGet(out liftGammaGain);
        liftGammaGain.gamma.value = new Vector4(0, 0, 0, currentGamma / 10f);
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
                targetIdx = 2;
                break;
            case "General":
                targetIdx = 3;
                break;
            case "Graphic":
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

    public void ApplySensitivityValue(string axis)
    {
        float val;
        if (axis == "x")
        {
            val = xSensitivitySlider.value;
            xSensitivityText.text = string.Format("{0:0.##}", val);
            PlayerPrefs.SetFloat("XSensitivityValue", val);
            return;
        }
        else if (axis == "y")
        {
            val = ySensitivitySlider.value;
            ySensitivityText.text = string.Format("{0:0.##}", val);
            PlayerPrefs.SetFloat("YSensitivityValue", val);
            return;
        }
        else
        {
            return;
        }
    }

    public void ResetSensitivityValues()
    {
        float val = 2f;
        xSensitivitySlider.value = val;
        PlayerPrefs.SetFloat("XSensitivityValue", val);
        xSensitivityText.text = string.Format("{0:0.##}", val);

        ySensitivitySlider.value = val;
        PlayerPrefs.SetFloat("YSensitivityValue", val);
        ySensitivityText.text = string.Format("{0:0.##}", val);
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

    public void ApplyKeyInfoPanelValue()
    {
        if (keyInfoPanelToggle.isOn)
        {
            PlayerPrefs.SetInt("DisableKeyInfoPanel", 1);
        }
        else
        {
            PlayerPrefs.SetInt("DisableKeyInfoPanel", 0);
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

    public void SensitivityTest()
    {
        StartCoroutine(DoSensitivityTest());
    }

    IEnumerator DoSensitivityTest()
    {
        float sensitivityX = PlayerPrefs.GetFloat("XSensitivityValue", 2f);
        float sensitivityY = PlayerPrefs.GetFloat("YSensitivityValue", 2f);

        Quaternion camRotation = sensitivityPreviewCam.rotation;
        Quaternion bodyRotation = sensitivityPreviewBody.rotation;

        Quaternion camOriginal = camRotation;
        Quaternion bodyOriginal = bodyRotation;


        Cursor.lockState = CursorLockMode.Locked;
        SetSensitivityPreviewText(true);

        yield return null; //정지 후 재개시 마우스 커서 좌표의 변화에 따라 카메라가 회전하므로 한 프레임 스킵

        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SetSensitivityPreviewText(false);
                Cursor.lockState = CursorLockMode.None;

                sensitivityPreviewCam.localRotation = camOriginal;
                sensitivityPreviewBody.localRotation = bodyOriginal;

                yield break;
            }

            float yRotation = Input.GetAxis("Mouse X") * sensitivityX;
            float xRotation = Input.GetAxis("Mouse Y") * sensitivityY;

            camRotation *= Quaternion.Euler(-xRotation, 0f, 0f);
            camRotation = ClampRotationAroundXAxis(camRotation);
            bodyRotation *= Quaternion.Euler(0f, yRotation, 0f);

            sensitivityPreviewCam.localRotation = camRotation;
            sensitivityPreviewBody.localRotation = bodyRotation;

            yield return null;
        }
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, -90f, 90f);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

    void SetSensitivityPreviewText(bool isActivated)
    {
        if (isActivated)
        {
            sensitivityPreviewText.text = "클릭하여 그만두기";
        }
        else
        {
            sensitivityPreviewText.text = "여기를 눌러 감도 체험하기";
        }
    }

    public void IncreaseGamma()
    {
        if (currentGamma >= 10) return;

        PlayerPrefs.SetFloat("GammaValue", ++currentGamma);
        gammaAmountText.text = currentGamma.ToString("+#;-#;0");
        liftGammaGain.gamma.value = new Vector4(0, 0, 0, currentGamma / 10f);
    }

    public void DecreaseGamma()
    {
        if (currentGamma <= -10) return;

        PlayerPrefs.SetFloat("GammaValue", --currentGamma);
        gammaAmountText.text = currentGamma.ToString("+#;-#;0");
        liftGammaGain.gamma.value = new Vector4(0, 0, 0, currentGamma / 10f);
    }
}
