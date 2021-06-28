using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIController : MonoBehaviour
{
    PlayerMovement player;
    ItemHolder itemHolder;
    [SerializeField] Text usbStatus;

    public Image[] staminaGaugaes;

    // Start is called before the first frame update
    void Start()
    {
        itemHolder = ItemHolder.Instance;
        player = PlayerMovement.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateItemStatusText();
        foreach (var e in staminaGaugaes)
        {
            e.fillAmount = player.currentStamina / player.maxStamina;
        }
    }

    void UpdateItemStatusText()
    {
        usbStatus.text = "USB º“¿Ø: " + itemHolder.HasUSBItem.ToString();
    }
}
