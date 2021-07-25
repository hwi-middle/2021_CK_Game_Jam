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
        foreach(var source in SE)
        {
            source.volume = PlayerPrefs.GetFloat("SEValue", 1f);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0)
            || Input.GetKeyDown(KeyCode.Space)
            || Input.GetKeyDown(KeyCode.Return)
            || Input.GetKeyDown(KeyCode.RightArrow))
        {
            idx++;
        }
        else if (prev == idx || idx > 16) return;

        //√©≈Õ ¿Ãµø
        if (idx == 6)
        {
            chapters[0].SetActive(false);
            chapters[1].SetActive(true);
        }
        else if (idx == 12)
        {
            chapters[1].SetActive(false);
            chapters[2].SetActive(true);
        }

        //¿Ã∆Â∆Æ ¿˚øÎ
        if (idx == 6)
        {
            fx[0].gameObject.SetActive(true);
        }
        else if (idx == 11)
        {
            fx[1].gameObject.SetActive(true);
        }
        else if (idx == 13)
        {
            BGM.pitch = 0.5f;
            SE[0].Play();
        }
        else if (idx == 14)
        {
            fx[2].gameObject.SetActive(true);
            SE[1].Play();
        }

        //æ¿ ¿Ãµø
        if (idx == 16)
        {
            chapters[2].SetActive(false);
            SE[0].Pause();
            SE[1].Pause();
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
        while(SE[2].isPlaying)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("2ndFloor");
    }
}
