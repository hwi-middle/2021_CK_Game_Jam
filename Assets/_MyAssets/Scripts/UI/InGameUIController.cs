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

    private bool isMemoPanelOpened = false;

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
        UpdateItemStatusText();
        float cutoff = 1 - (player.currentStamina / player.maxStamina);
        //staminaPlane.GetComponent<Renderer>().sharedMaterial.SetFloat("_Cutoff", cutoff);

        staminaGuage.fillAmount = player.currentStamina / player.maxStamina;

        if (Input.GetKey(KeyCode.Z))
        {
            ControlMemoPanel();
        }

        if(isMemoPanelOpened)
        {
        }
    }

    void UpdateItemStatusText()
    {
        USBIcon.sprite = USBSprites[itemHolder.ItemIndex];
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
