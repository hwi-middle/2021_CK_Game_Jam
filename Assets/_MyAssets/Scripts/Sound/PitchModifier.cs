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
            //ü���� ���������� pitch ���߱�
            audioSource.pitch = Mathf.Lerp(1.0f, pitchMinValue, player.currentHealth / player.maxHealth);
        }
        else
        {
            //ȸ������ ���� ��ġ������ ü�°��� Ŀ���� ȿ�� off
            audioSource.pitch = 1.0f;
        }
    }
}
