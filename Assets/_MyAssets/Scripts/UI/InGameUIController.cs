using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIController : MonoBehaviour
{
    private PlayerMovement player;
    private ItemHolder itemHolder;

    public GameObject staminaPlane;
    public Image staminaGuage;
    public Image USBIcon;
    public Sprite[] USBSprites;
    public Text healthDebugText;

    private bool isMemoPanelOpened = false;
    private bool isStopped = false;

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

        //Ű�Է��� �켱�������� 1���� �ޱ�
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isStopped)
            {
                isStopped = false;
                player.shouldCameraFreeze = false;
                Debug.Log("���� �簳");
                Time.timeScale = 1f;
            }
            else
            {
                isStopped = true;
                player.shouldCameraFreeze = true;
                Debug.Log("���� ����");
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
