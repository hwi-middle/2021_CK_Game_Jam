using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIController : MonoBehaviour
{
    ItemHolder itemHolder;
    [SerializeField] Text usbStatus;

    // Start is called before the first frame update
    void Start()
    {
        itemHolder = ItemHolder.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateItemStatusText();
    }

    void UpdateItemStatusText()
    {
        usbStatus.text = "USB º“¿Ø: " + itemHolder.HasUSBItem.ToString();
    }
}
