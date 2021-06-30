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
        float cutoff = 1 - (player.currentStamina / player.maxStamina);
        staminaPlane.GetComponent<Renderer>().sharedMaterial.SetFloat("_Cutoff", cutoff);

        //Image로 처리했을 때 남은 코드
        //foreach (var e in staminaGaugaes)
        //{
        //    e.fillAmount = player.currentStamina / player.maxStamina;
        //}
    }

    void UpdateItemStatusText()
    {
        usbStatus.text = "USB 소유: " + itemHolder.HasUSBItem.ToString();
    }
}
