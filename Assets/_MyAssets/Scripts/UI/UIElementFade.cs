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


    void Start()
    {
        gameObject.SetActive(true);

        imgSrc = GetComponent<Image>();
        if (autoPlay)
        {
            Invoke("CallFadeCoroutine", startDelay);
        }
    }

    private void CallFadeCoroutine()
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
        Color color = imgSrc.color;
        while (imgSrc.color.a < 1f)
        {
            color.a += Time.deltaTime / t;
            imgSrc.color = color;
            yield return null;
        }
    }

    private IEnumerator FadeOut(float t)
    {
        Color color = imgSrc.color;
        while (imgSrc.color.a > 0f)
        {
            color.a -= Time.deltaTime / t;
            imgSrc.color = color;
            yield return null;
        }
    }

    private IEnumerator FadeOutAndFadeIn(float t1, float t2, float delay)
    {
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
    }

    private IEnumerator FadeInAndFadeOut(float t1, float t2, float delay)
    {
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
    }

    private IEnumerator FadeInAndLoadScene(float t, string name)
    {
        Color color = imgSrc.color;
        while (imgSrc.color.a < 1f)
        {
            color.a += Time.deltaTime / t;
            imgSrc.color = color;
            yield return null;
        }

        SceneManager.LoadScene(name);
    }

    public void LoadSceneAfterBlackout(string str)
    {
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
