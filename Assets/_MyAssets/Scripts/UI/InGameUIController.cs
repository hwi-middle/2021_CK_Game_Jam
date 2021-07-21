using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIController : MonoBehaviour
{
    private PlayerMovement player;
    private ItemHolder itemHolder;

    [SerializeField] private GameObject staminaPlane;
    [SerializeField] private Image staminaGuage;
    [SerializeField] private Image USBIcon;
    [SerializeField] private Sprite[] USBSprites;
    [SerializeField] private Text coinText;
    [SerializeField] private Image healthItemIcon;
    [SerializeField] private Sprite[] healthItemSprites;
    [SerializeField] private Text healthDebugText;

    private bool isMemoPanelOpened = false;
    private bool isStopped = false;

    [SerializeField] private Canvas inGameCanvas;
    [SerializeField] private Canvas pauseCanvas;

    [SerializeField] private AudioSource bgm;

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
        UpdateUSBItemStatus();
        UpdateHealthStatus();
        UpdateHealthItemStatus();
        UpdateCoinAmount();

        float cutoff = 1 - (player.currentStamina / player.maxStamina);
        //staminaPlane.GetComponent<Renderer>().sharedMaterial.SetFloat("_Cutoff", cutoff);

        staminaGuage.fillAmount = player.currentStamina / player.maxStamina;

        //키입력은 우선순위별로 1개만 받기
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isStopped)
            {
                bgm.Play();
                player.SetCursorLockState(CursorLockMode.Locked);
                inGameCanvas.gameObject.SetActive(true);
                pauseCanvas.gameObject.SetActive(false);
                isStopped = false;
                player.shouldCameraFreeze = false;
                Debug.Log("게임 재개");
                Time.timeScale = 1f;
            }
            else
            {
                bgm.Pause();
                player.SetCursorLockState(CursorLockMode.None);
                inGameCanvas.gameObject.SetActive(false);
                pauseCanvas.gameObject.SetActive(true);
                isStopped = true;
                player.shouldCameraFreeze = true;
                Debug.Log("게임 정지");
                Time.timeScale = 0f;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            ControlMemoPanel();
        }
        else if (Input.GetKeyDown(KeyCode.Q) && !player.isDead)
        {
            if (itemHolder.HasHealthItem)
            {
                itemHolder.HasHealthItem = false;

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

    void UpdateHealthStatus()
    {
        healthDebugText.text = player.currentHealth.ToString() + "%";
    }

    void UpdateCoinAmount()
    {
        coinText.text = "x" + itemHolder.Coin.ToString();
    }

    void ControlMemoPanel()
    {
        if (isMemoPanelOpened)
        {

        }
        else
        {

        }
    }
}
