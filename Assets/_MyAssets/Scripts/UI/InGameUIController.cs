using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIController : MonoBehaviour
{
    PlayerMovement player;
    ItemHolder itemHolder;
    [SerializeField] Text usbStatus;

    public GameObject staminaPlane;
    public Image USBIcon;
    public Sprite[] USBSprites;

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
        staminaPlane.GetComponent<Renderer>().sharedMaterial.SetFloat("_Cutoff", cutoff);

        //Image�� ó������ �� ���� �ڵ�
        //foreach (var e in staminaGaugaes)
        //{
        //    e.fillAmount = player.currentStamina / player.maxStamina;
        //}
    }

    void UpdateItemStatusText()
    {
        USBIcon.sprite = USBSprites[itemHolder.ItemIndex];
        usbStatus.text = "USB ����: " + itemHolder.HasUSBItem.ToString();
    }
}
