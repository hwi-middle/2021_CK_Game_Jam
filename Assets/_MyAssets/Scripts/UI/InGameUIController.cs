using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameUIController : MonoBehaviour
{
    private PlayerMovement player;
    private ItemHolder itemHolder;

    [SerializeField] private IngameObjectsManager ingameObjectsManager;
    [SerializeField] private SceneResetManager sceneResetManager;

    [SerializeField] private GameObject KeyInfoPanel1;
    [SerializeField] private GameObject KeyInfoPanel2;

    [SerializeField] private Image staminaGuage;
    [SerializeField] private Image USBIcon;
    [SerializeField] private Sprite[] USBSprites;
    [SerializeField] private Text coinText;
    [SerializeField] private Image healthItemIcon;
    [SerializeField] private Sprite[] healthItemSprites;
    //[SerializeField] private Text healthDebugText;
    [SerializeField] private Text stunText;
    [SerializeField] private Text curseText;

    private ECanvasType currentCanvas = ECanvasType.None;
    [SerializeField] private Canvas inGameCanvas;
    [SerializeField] private Canvas pauseCanvas;
    [SerializeField] private Canvas mapCanvas;
    [SerializeField] private Canvas dieCanvas;
    [SerializeField] private Canvas tutorialCanvas;

    [SerializeField] private Image dieImage;
    [SerializeField] private Sprite[] dieSprites;
    [SerializeField] private Button dieButton;

    [SerializeField] private AudioSource bgm;
    [SerializeField] private AudioSource beerCanSound;
    [SerializeField] private AudioSource outOfHealthSound;
    [SerializeField] private AudioSource dieSound;
    [SerializeField] private EnemyDetector enemyDetector;

    // Start is called before the first frame update
    void Start()
    {
        USBIcon.sprite = USBSprites[0];
        itemHolder = ItemHolder.Instance;
        player = PlayerMovement.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isDead && !player.isDying)
        {
            player.isDying = true;
            StartCoroutine(Die());
            return;
        }

        UpdateUSBItemStatus();
        //UpdateHealthStatus();
        UpdateHealthItemStatus();
        UpdateCoinAmount();
        UpdateKeyInfoPanel();
        UpdateCurseAndStunText();

        float cutoff = 1 - (player.currentStamina / player.maxStamina);
        //staminaPlane.GetComponent<Renderer>().sharedMaterial.SetFloat("_Cutoff", cutoff);

        staminaGuage.fillAmount = player.currentStamina / player.maxStamina;

        if (player.doingTask)
        {
            return;
        }

        //키입력은 우선순위별로 1개만 받기
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (player.isDead || player.isStunned || player.isStunInvincible || player.isCursed || player.isCurseInvincible)
            {
                return;
            }

            if (currentCanvas == ECanvasType.Pause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Q) && !player.isDead)
        {
            if (itemHolder.HasHealthItem)
            {
                itemHolder.HasHealthItem = false;
                beerCanSound.Play();
                int healAmount = 0;
                switch (itemHolder.HealthItemType)
                {
                    case EItemType.MonsterEnergy:
                        healAmount = 50;
                        break;
                    case EItemType.TomatoJuice:
                        healAmount = 25;
                        break;
                    case EItemType.CucumberJuice:
                        healAmount = 15;
                        break;
                    default:
                        Debug.Assert(false);
                        break;
                }
                player.Heal(healAmount);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (currentCanvas == ECanvasType.Map)
            {
                currentCanvas = ECanvasType.None;
                mapCanvas.gameObject.SetActive(false);
            }
            else if (currentCanvas == ECanvasType.None)
            {
                currentCanvas = ECanvasType.Map;
                mapCanvas.gameObject.SetActive(true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentCanvas == ECanvasType.Tutorial)
            {
                currentCanvas = ECanvasType.None;
                tutorialCanvas.gameObject.SetActive(false);
            }
            else if (currentCanvas == ECanvasType.None)
            {
                currentCanvas = ECanvasType.Tutorial;
                tutorialCanvas.gameObject.SetActive(true);
            }
        }
    }

    IEnumerator Die()
    {
        Time.timeScale = 0f;

        switch (player.dieType)
        {
            case EDieType.Chased:
                dieImage.sprite = dieSprites[0];
                break;
            case EDieType.Stunned:
                dieImage.sprite = dieSprites[1];
                break;
            case EDieType.TimeOver:
                dieImage.sprite = dieSprites[2];
                break;
            case EDieType.None:
            default:
                Debug.Assert(false);
                break;
        }

        CloseAllCanvas();
        player.SetCursorLockState(CursorLockMode.None);
        outOfHealthSound.Play();
        yield return new WaitForSecondsRealtime(4.0f);
        outOfHealthSound.Stop();
        enemyDetector.StopSE();
        dieSound.Play();
        dieCanvas.gameObject.SetActive(true);
        dieButton.gameObject.SetActive(true);

        yield break;
    }

    public void CloseAllCanvas()
    {
        mapCanvas.gameObject.SetActive(false);
        tutorialCanvas.gameObject.SetActive(false);
    }

    void UpdateUSBItemStatus()
    {
        USBIcon.sprite = USBSprites[itemHolder.USBItemIndex];
    }

    void UpdateHealthItemStatus()
    {
        if (!itemHolder.HasHealthItem)
        {
            healthItemIcon.sprite = healthItemSprites[0]; //0번째 요소는 기본 이미지
            return;
        }

        healthItemIcon.sprite = healthItemSprites[(int)itemHolder.HealthItemType + 1];
    }

    //void UpdateHealthStatus()
    //{
    //    healthDebugText.text = player.currentHealth.ToString() + "%";
    //}

    void UpdateCoinAmount()
    {
        coinText.text = "x" + itemHolder.Coin.ToString();
    }

    void UpdateKeyInfoPanel()
    {
        if (PlayerPrefs.GetInt("DisableKeyInfoPanel", 0) == 1)
        {
            KeyInfoPanel1.SetActive(false);
            KeyInfoPanel2.SetActive(false);
        }
        else
        {
            KeyInfoPanel1.SetActive(true);
            KeyInfoPanel2.SetActive(true);
        }
    }

    void UpdateCurseAndStunText()
    {
        if (player.isCursed)
        {
            curseText.gameObject.SetActive(true);
        }
        else
        {
            curseText.gameObject.SetActive(false);
        }

        if (player.isStunned)
        {
            stunText.gameObject.SetActive(true);
        }
        else
        {
            stunText.gameObject.SetActive(false);
        }
    }

    void Pause()
    {
        currentCanvas = ECanvasType.Pause;
        bgm.Pause();
        player.SetCursorLockState(CursorLockMode.None);
        inGameCanvas.gameObject.SetActive(false);
        mapCanvas.gameObject.SetActive(false);
        pauseCanvas.gameObject.SetActive(true);
        player.shouldCameraFreeze = true;
        Debug.Log("게임 정지");
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        currentCanvas = ECanvasType.None;
        bgm.Play();
        player.SetCursorLockState(CursorLockMode.Locked);
        inGameCanvas.gameObject.SetActive(true);
        pauseCanvas.gameObject.SetActive(false);
        player.shouldCameraFreeze = false;
        Debug.Log("게임 재개");
        Time.timeScale = 1f;
    }

    public void MoveToSettingsScene()
    {
        ingameObjectsManager.Deactivate();
        PlayerPrefs.SetInt("IsFromIngame", 1);
        SceneManager.LoadScene("Settings");
    }

    public void ReloadScene()
    {
        sceneResetManager.ClearAllObjectsAndLoadScene("2ndFloor");
    }

    public void ReturnToLobbyScene()
    {
        sceneResetManager.ClearAllObjectsAndLoadScene("Lobby");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
