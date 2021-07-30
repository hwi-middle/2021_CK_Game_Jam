using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CorrectPasswordBGMPlayer : MonoBehaviour
{
    private PlayerMovement player;
    private AudioSource audioSource;
    [SerializeField] private AudioClip beep;
    [SerializeField] private AudioClip type;
    [SerializeField] private AudioClip bgm;

    [SerializeField] private Canvas passwordCanvas;
    [SerializeField] private Text title;
    [SerializeField] private Text log;
    private string logString1 = "<Start Debugging>\n*** Smart Debugger v1.12.89b ***\n";
    private string logString2 = "Fatal error: 'debuglib.h' - No such file or directory. Unable to start program.\nRecovering...\n";
    private string logString3 = "Warning: An unrecoverable error has occurred. (Unhandled Exception: 0xc0000005)\nWarning: An unknown error occurred while transferring data\n";
    private string logString4 = "Where are you going?";

    [SerializeField] private Canvas codeCanvas;
    [SerializeField] Volume volume;
    private LimitlessGlitch8 glitch1;
    private LimitlessGlitch12 glitch2;
    [SerializeField] private AudioClip endHitClip;
    [SerializeField] private Image cover;

    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGet(out glitch1);
        volume.profile.TryGet(out glitch2);

        player = PlayerMovement.Instance;
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetFloat("SEValue", 1f);

        StartCoroutine(ControlMusicAndScene());
        glitch1.enable.value = false;
        glitch2.enable.value = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator ControlMusicAndScene()
    {
        player.shouldCameraFreeze = true;
        player.shouldMoveFreeze = true;
        audioSource.clip = beep;
        audioSource.Play();
        title.text = "암호 일치";
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < 3; i++)
        {
            title.text = "";
            yield return new WaitForSeconds(0.5f);
            title.text = "암호 일치";
            audioSource.Play();
            yield return new WaitForSeconds(0.5f);
        }

        audioSource.clip = type;
        audioSource.volume = PlayerPrefs.GetFloat("SEValue", 1f) * 0.5f;
        for (int i = 0; i < logString1.Length; i++)
        {
            log.text += logString1[i];
            if (logString1[i] == '\n' || logString1[i] == '\t')
            {
                yield return new WaitForSeconds(0.5f);
            }
            audioSource.Play();
            yield return new WaitForSeconds(0.035f);
        }
        yield return new WaitForSeconds(2f);
        title.text = "ERROR";
        title.color = Color.red;
        title.fontSize = 110;

        for (int i = 0; i < logString2.Length; i++)
        {
            log.text += logString2[i];
            if (logString2[i] == '\n' || logString2[i] == '\t')
            {
                yield return new WaitForSeconds(1.2f);
            }
            audioSource.Play();
            yield return new WaitForSeconds(0.035f);
        }

        yield return new WaitForSeconds(1f);
        for (int i = 0; i < logString3.Length; i++)
        {
            log.text += logString3[i];
            if (logString3[i] == '\n' || logString3[i] == '\t')
            {
                yield return new WaitForSeconds(0.2f);
            }
            audioSource.Play();
            yield return new WaitForSeconds(0.035f);
        }

        yield return new WaitForSeconds(2f);
        string dots = "...";
        for (int i = 0; i < dots.Length; i++)
        {
            log.text += dots[i];
            if (dots[i] == '\n' || dots[i] == '\t')
            {
                yield return new WaitForSeconds(0.2f);
            }
            audioSource.Play();
            yield return new WaitForSeconds(0.035f);
        }

        yield return new WaitForSeconds(2f);
        for (int i = 0; i < logString4.Length; i++)
        {
            log.text += logString4[i];
            if (logString4[i] == '\n' || logString4[i] == '\t')
            {
                yield return new WaitForSeconds(0.2f);
            }
            audioSource.Play();
            yield return new WaitForSeconds(0.035f);
        }

        yield return new WaitForSeconds(2f);
        if (PlayerPrefs.GetInt("DisableVFX") != 1)
        {
            glitch1.enable.value = true;
            glitch2.enable.value = true;
        }
        for (int i = 0; i < 100; i++)
        {
            int cnt = 0;
            for (int j = 0; j < logString4.Length; j++)
            {
                log.text += logString4[j];
                if (logString4[j] == '\n' || logString4[j] == '\t')
                {
                    yield return new WaitForSeconds(0.2f);
                }
                else if(cnt < 3)
                {
                    cnt++;
                    continue;
                }
                cnt = 0;
                audioSource.Play();
                yield return new WaitForSeconds(0.000001f);
            }
        }

        yield return new WaitForSeconds(0.2f);

        passwordCanvas.gameObject.SetActive(false);
        codeCanvas.gameObject.SetActive(true);
        player.shouldCameraFreeze = false;
        player.shouldMoveFreeze = false;
        audioSource.clip = bgm;
        audioSource.volume = PlayerPrefs.GetFloat("BGMValue", 1f);
        audioSource.Play();
        while (audioSource.isPlaying)
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
        player.shouldMoveFreeze = true;
        glitch1.enable.value = false;
        glitch2.enable.value = false;
        cover.gameObject.SetActive(true);
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        player.SetCursorLockState(CursorLockMode.None);
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("Epilogue");
    }
}
