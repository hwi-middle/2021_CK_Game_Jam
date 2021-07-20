using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchModifier : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [Range(0f, 100f)]
    [SerializeField] private float effectStartThreshold = 80f;
    [Range(0.1f, 1f)]
    [SerializeField] private float pitchMinValue = 0.5f;
    private PlayerMovement player;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerMovement.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("DisableVFX") == 1) return;

        if (player.currentHealth <= effectStartThreshold)
        {
            Debug.Log("dd");
            //체력이 낮아질수록 pitch 낮추기
            audioSource.pitch = Mathf.Lerp(1.0f, pitchMinValue, player.currentHealth / player.maxHealth);
        }
        else
        {
            //회복으로 인해 역치값보다 체력값이 커지면 효과 off
            audioSource.pitch = 1.0f;
        }
    }
}
