using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundVolumeModifier : MonoBehaviour
{
    private AudioSource audioSource;
    public ESoundType type;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if(type == ESoundType.BGM && audioSource.playOnAwake ==false)
        {
            audioSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (type)
        {
            case ESoundType.BGM:
                audioSource.volume = PlayerPrefs.GetFloat("BGMValue", 1f);
                break;
            case ESoundType.SE:
                audioSource.volume = PlayerPrefs.GetFloat("SEValue", 1f);
                break;
            case ESoundType.Footstep:
                audioSource.volume = PlayerPrefs.GetFloat("FootstepSoundValue", 1f);
                break;
            default:
                Debug.Assert(false);
                break;
        }
    }
}
