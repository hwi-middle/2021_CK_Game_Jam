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
    public float delay;
    public float duration;
    private Image imgSrc;


    void Start()
    {
        gameObject.SetActive(true);

        imgSrc = GetComponent<Image>();
        if (autoPlay)
        {
            Invoke("CallFadeCoroutine", delay);
        }
    }

    private void CallFadeCoroutine()
    {
        switch (fadeType)
        {
            case EFadeType.FadeIn:
                StartCoroutine(FadeIn(duration));
                break;
            case EFadeType.FadeOut:
                StartCoroutine(FadeOut(duration));
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

    public void FadeInAndLoadScene(string str)
    {
        string[] res = str.Split(new char[] { ',', ' '} , StringSplitOptions.RemoveEmptyEntries);

        foreach(var i in res)
        {
            Debug.Log(i);
        }
        float duration = float.Parse(res[0]);
        string targetSceneName = res[1];


        StartCoroutine(FadeIn(duration));
        SceneManager.LoadScene(targetSceneName);
    }
}
