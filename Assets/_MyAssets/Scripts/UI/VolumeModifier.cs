using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeModifier : MonoBehaviour
{
    [SerializeField] private Volume volume;
    [Range(0f,100f)]
    [SerializeField] private float effectStartThreshold = 90f;
    [Range(0f, 80f)]
    [SerializeField] private float healthGlitchMaxValue = 20f;
    private LimitlessGlitch6 healthGlitch;
    private LimitlessGlitch8 gameOverGlitch;
    private PlayerMovement player;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerMovement.Instance;
        volume.profile.TryGet(out healthGlitch);
        volume.profile.TryGet(out gameOverGlitch);
        healthGlitch.enable.value = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.isDead)
        {
            healthGlitch.enable.value = false;
            gameOverGlitch.enable.value = true;
        }
        else if(player.currentHealth <= effectStartThreshold)
        {       
            //Ã¼·ÂÀÌ ³·¾ÆÁú¼ö·Ï amount°ªÀº ÃÖ´ñ°ª¿¡ ¼ö·Å
            healthGlitch.amount.value = healthGlitchMaxValue * (1 - (player.currentHealth / player.maxHealth));
        }
    }
}
