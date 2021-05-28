using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EFadeType
{
    FadeIn,
    FadeOut
}

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
        if(autoPlay)
        {
            Invoke("CallFadeCoroutine", delay);
        }
    }

    private void CallFadeCoroutine()
    {
        switch (fadeType)
        {
            case EFadeType.FadeIn:
                StartCoroutine("FadeIn");
                break;
            case EFadeType.FadeOut:
                StartCoroutine("FadeOut");
                break;
            default:
                Debug.Assert(false);
                break;
        }
    }

    private IEnumerator FadeIn()
    {
        Color color = imgSrc.color;
        while (imgSrc.color.a < 1f)
        {
            color.a += Time.deltaTime / duration;
            imgSrc.color = color;
            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        Color color = imgSrc.color;
        while (imgSrc.color.a > 0f)
        {
            color.a -= Time.deltaTime / duration;
            imgSrc.color = color;
            yield return null;
        }
    }
}
