using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CorrectPasswordBGMPlayer : MonoBehaviour
{
    private PlayerMovement player;
    private AudioSource audioSource;
    [SerializeField] private AudioClip endHitClip;
    [SerializeField] private Image cover;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerMovement.Instance;
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(ControlMusicAndScene());
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
