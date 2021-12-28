using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EpilogueController : MonoBehaviour
{
    private int prev = -1;
    private int idx = 0;
    [SerializeField] private GameObject cartoon;
    [SerializeField] private Image[] cartoonImages;

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
        if (idx > 5) return;

        if (Input.GetMouseButtonDown(0)
            || Input.GetKeyDown(KeyCode.Space)
            || Input.GetKeyDown(KeyCode.Return)
            || Input.GetKeyDown(KeyCode.RightArrow))
        {
            idx++;
        }
        else if (prev == idx) return;

        //효과음 적용
        if (idx == 2)
        {
            SE[0].Play();
        }
        if (idx == 5)
        {
            SE[1].Play();
        }

        //씬 이동
        if (idx == 6)
        {
            cartoon.SetActive(false);
            SE[2].Play();
            prev = idx;
            StartCoroutine(WaitAndLoadScene());
            return;
        }

        cartoonImages[idx].gameObject.SetActive(true);
        prev = idx;
    }

    IEnumerator WaitAndLoadScene()
    {
        while (SE[2].isPlaying)
        {
            yield return null;
        }

        float volume = 1f;
        while (BGM.volume > 0)
        {
            volume -= 1f * Time.deltaTime;
            if (volume < 0) volume = 0;
            BGM.volume = volume;
            Debug.Log(BGM.volume);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Credit");
    }
}
