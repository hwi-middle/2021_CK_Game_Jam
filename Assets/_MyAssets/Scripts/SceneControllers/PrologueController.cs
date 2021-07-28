using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PrologueController : MonoBehaviour
{
    private int prev = -1;
    private int idx = 0;
    [SerializeField] private GameObject[] chapters;
    [SerializeField] private Image[] cartoonImages;
    [SerializeField] private Image[] fx;

    [SerializeField] private AudioSource BGM;
    [SerializeField] private AudioSource[] SE;

    // Start is called before the first frame update
    void Start()
    {
        BGM.volume = PlayerPrefs.GetFloat("BGMValue", 1f);
        foreach (var source in SE)
        {
            source.volume = PlayerPrefs.GetFloat("SEValue", 1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(idx > 24) return;

        if (Input.GetMouseButtonDown(0)
            || Input.GetKeyDown(KeyCode.Space)
            || Input.GetKeyDown(KeyCode.Return)
            || Input.GetKeyDown(KeyCode.RightArrow))
        {
            idx++;
        }
        else if (prev == idx) return;

        //Ã©ÅÍ ÀÌµ¿
        if (idx == 8)
        {
            chapters[0].SetActive(false);
            chapters[1].SetActive(true);
        }
        else if (idx == 14)
        {
            chapters[1].SetActive(false);
            chapters[2].SetActive(true);
        }
        else if (idx == 20)
        {
            chapters[2].SetActive(false);
            chapters[3].SetActive(true);
        }

        //ÀÌÆåÆ® Àû¿ë
        if (idx == 2)
        {
            SE[0].Play();
        }
        else if (idx == 5)
        {
            SE[1].Play();
        }
        else if (idx == 7)
        {
            fx[0].gameObject.SetActive(true);
        }
        else if (idx == 14)
        {
            fx[1].gameObject.SetActive(true);
        }
        else if (idx == 19)
        {
            fx[2].gameObject.SetActive(true);
        }
        else if (idx == 21)
        {
            BGM.pitch = 0.5f;
            SE[2].Play();
        }
        else if (idx == 22)
        {
            fx[3].gameObject.SetActive(true);
            SE[3].Play();
        }

        //¾À ÀÌµ¿
        if (idx == 24)
        {
            chapters[3].SetActive(false);
            SE[2].Pause();
            SE[3].Pause();
            SE[4].Play();
            prev = idx;
            StartCoroutine(WaitAndLoadScene());
            return;
        }

        cartoonImages[idx].gameObject.SetActive(true);
        prev = idx;
    }

    IEnumerator WaitAndLoadScene()
    {
        while (SE[4].isPlaying)
        {
            yield return null;
        }

        float volume = 1f;
        while (BGM.volume > 0)
        {
            volume -= 1f * Time.deltaTime;
            if (volume < 0) volume = 0;
            BGM.volume = volume;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Tutorial");
    }
}
