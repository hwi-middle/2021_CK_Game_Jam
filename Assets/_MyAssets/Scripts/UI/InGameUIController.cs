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
    [SerializeField] private Text healthDebugText;

    private bool isMemoPanelOpened = false;
    private bool isStopped = false;

    [SerializeField] private Canvas inGameCanvas;
    [SerializeField] private Canvas pauseCanvas;

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
        UpdateItemStatus();
        UpdateHealthStatus();
        float cutoff = 1 - (player.currentStamina / player.maxStamina);
        //staminaPlane.GetComponent<Renderer>().sharedMaterial.SetFloat("_Cutoff", cutoff);

        staminaGuage.fillAmount = player.currentStamina / player.maxStamina;

        //키입력은 우선순위별로 1개만 받기
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isStopped)
            {
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

        if(isMemoPanelOpened)
        {
        }
    }

    void UpdateItemStatus()
    {
        USBIcon.sprite = USBSprites[itemHolder.ItemIndex];
    }

    void UpdateHealthStatus()
    {
        healthDebugText.text = player.currentHealth.ToString() + "%";
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
