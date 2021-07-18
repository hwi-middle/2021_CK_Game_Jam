using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoHandler : MonoBehaviour
{
    [SerializeField] private RawImage screen;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private UIElementFade fade;

    void Start()
    {
        StartCoroutine(PrepareVideo());
    }

    protected IEnumerator PrepareVideo()
    {
        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }
        screen.texture = videoPlayer.texture;

        PlayVideo();
        fade.CallFadeCoroutine();
    }

    public void PlayVideo()
    {
        if (videoPlayer.isPrepared)
        {
            videoPlayer.Play();
        }
    }

    public void StopVideo()
    {
        if (videoPlayer.isPrepared)
        {
            videoPlayer.Stop();
        }
    }
}