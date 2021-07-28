using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CorrectPasswordBGMPlayer : MonoBehaviour
{
    private PlayerMovement player;
    private AudioSource audioSource;
    [SerializeField] Volume volume;
    private LimitlessGlitch8 glitch1;
    private LimitlessGlitch12 glitch2;
    [SerializeField] private AudioClip endHitClip;
    [SerializeField] private Image cover;

    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGet(out glitch1);
        volume.profile.TryGet(out glitch2);

        player = PlayerMovement.Instance;
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(ControlMusicAndScene());

        if (PlayerPrefs.GetInt("DisableVFX") == 1)
        {
            glitch1.enable.value = false;
            glitch2.enable.value = false;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator ControlMusicAndScene()
    {
        while(audioSource.isPlaying)
        {
            yield return null;
        }
        audioSource.Play();
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        audioSource.clip = endHitClip;
        audioSource.Play();
        player.shouldMoveFreeze = true;
        cover.gameObject.SetActive(true);
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        player.SetCursorLockState(CursorLockMode.None);
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("Epilogue");
    }
}
