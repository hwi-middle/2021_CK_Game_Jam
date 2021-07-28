using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CorrectPasswordBGMPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    private int count = 0;
    [SerializeField] private AudioClip endHitClip;
    [SerializeField] private Image cover;

    // Start is called before the first frame update
    void Start()
    {
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
        cover.gameObject.SetActive(true);
    }
}
