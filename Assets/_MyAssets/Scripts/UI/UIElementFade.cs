using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIElementFade : MonoBehaviour
{
    public bool autoPlay = false;
    public EFadeType fadeType;
    public float startDelay;
    public float duration1;
    public float duration2;
    public float fadeDelay;
    private Image imgSrc;
    private bool isPlaying = false;


    void Awake()
    {
        gameObject.SetActive(true);

        imgSrc = GetComponent<Image>();
        if (autoPlay)
        {
            Invoke("CallFadeCoroutine", startDelay);
        }
    }

    public void CallFadeCoroutine()
    {
        switch (fadeType)
        {
            case EFadeType.FadeIn:
                StartCoroutine(FadeIn(duration1));
                break;
            case EFadeType.FadeOut:
                StartCoroutine(FadeOut(duration1));
                break;
            case EFadeType.FadeOutAndFadeIn:
                StartCoroutine(FadeOutAndFadeIn(duration1, duration2, fadeDelay));
                break;
            case EFadeType.FadeInAndFadeOut:
                break;
            default:
                Debug.Assert(false);
                break;
        }
    }

    private IEnumerator FadeIn(float t)
    {
        imgSrc.enabled = true;
        if (isPlaying) yield break;
        isPlaying = true;
        Color color = imgSrc.color;
        while (imgSrc.color.a < 1f)
        {
            color.a += Time.deltaTime / t;
            imgSrc.color = color;
            yield return null;
        }
        isPlaying = false;
    }

    private IEnumerator FadeOut(float t)
    {
        imgSrc.enabled = true;
        if (isPlaying) yield break;
        isPlaying = true;
        Color color = imgSrc.color;
        while (imgSrc.color.a > 0f)
        {
            color.a -= Time.deltaTime / t;
            imgSrc.color = color;
            yield return null;
        }
        imgSrc.enabled = false;
        isPlaying = false;
    }

    private IEnumerator FadeOutAndFadeIn(float t1, float t2, float delay)
    {
        imgSrc.enabled = true;
        if (isPlaying) yield break;
        isPlaying = true;
        Color color = imgSrc.color;
        while (imgSrc.color.a > 0f)
        {
            color.a -= Time.deltaTime / t1;
            imgSrc.color = color;
            yield return null;
        }

        yield return new WaitForSeconds(delay);

        while (imgSrc.color.a < 1f)
        {
            color.a += Time.deltaTime / t2;
            imgSrc.color = color;
            yield return null;
        }
        imgSrc.enabled = true;
        isPlaying = false;
    }

    private IEnumerator FadeInAndFadeOut(float t1, float t2, float delay)
    {
        imgSrc.enabled = true;
        if (isPlaying) yield break;
        isPlaying = true;
        Color color = imgSrc.color;
        while (imgSrc.color.a < 1f)
        {
            color.a += Time.deltaTime / t2;
            imgSrc.color = color;
            yield return null;
        }
        
        yield return new WaitForSeconds(delay);

        while (imgSrc.color.a > 0f)
        {
            color.a -= Time.deltaTime / t1;
            imgSrc.color = color;
            yield return null;
        }
        imgSrc.enabled = false;
        isPlaying = false;
    }

    private IEnumerator FadeInAndLoadScene(float t, string name)
    {
        imgSrc.enabled = true;
        if (isPlaying) yield break;
        isPlaying = true;
        Color color = imgSrc.color;
        while (imgSrc.color.a < 1f)
        {
            color.a += Time.deltaTime / t;
            imgSrc.color = color;
            yield return null;
        }

        isPlaying = false;
        SceneManager.LoadScene(name);
    }

    public void LoadSceneAfterBlackout(string str)
    {
        imgSrc.enabled = true;
        string[] res = str.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var i in res)
        {
            Debug.Log(i);
        }
        float duration = float.Parse(res[0]);
        string targetSceneName = res[1];

        StartCoroutine(FadeInAndLoadScene(duration, targetSceneName));
    }
}
