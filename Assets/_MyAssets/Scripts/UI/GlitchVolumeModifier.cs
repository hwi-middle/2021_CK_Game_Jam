using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GlitchVolumeModifier : MonoBehaviour
{
    [SerializeField] private Volume volume;
    [Range(0f, 100f)]
    [SerializeField] private float effectStartThreshold = 80f;
    [Range(0f, 80f)]
    [SerializeField] private float healthGlitchMaxValue = 20f;
    [SerializeField] private Light directionalLight;
    private LimitlessGlitch6 healthGlitch;
    private LimitlessGlitch8 gameOverGlitch;
    private LimitlessGlitch9 stunGlitch;
    private LimitlessGlitch17 curseGlitch;
    private PlayerMovement player;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerMovement.Instance;
        volume.profile.TryGet(out healthGlitch);
        volume.profile.TryGet(out gameOverGlitch);
        volume.profile.TryGet(out stunGlitch);
        volume.profile.TryGet(out curseGlitch);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("DisableVFX") == 1) return;

        if (player.isDead)
        {
            directionalLight.gameObject.SetActive(true);
            healthGlitch.enable.value = false;
            gameOverGlitch.enable.value = true;
            return;
        }
        else if (player.currentHealth <= effectStartThreshold)
        {
            healthGlitch.enable.value = true;
            //체력이 낮아질수록 amount값은 최댓값에 수렴
            healthGlitch.amount.value = healthGlitchMaxValue * (1 - (player.currentHealth / player.maxHealth));
        }
        else
        {
            //회복으로 인해 역치값보다 체력값이 커지면 효과 off
            healthGlitch.enable.value = false;
        }

        if (player.isStunned)
        {
            directionalLight.gameObject.SetActive(true);
            stunGlitch.enable.value = true;
        }
        else
        {
            directionalLight.gameObject.SetActive(false);
            stunGlitch.enable.value = false;
        }

        if(player.isCursed)
        {
            curseGlitch.enable.value = true;
        }
        else
        {
            curseGlitch.enable.value = false;
        }
    }
}
