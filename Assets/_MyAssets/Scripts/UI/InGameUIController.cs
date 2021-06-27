using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIController : MonoBehaviour
{
    [SerializeField] ItemHolder itemHolder;
    [SerializeField] Text usbStatus;

    // Start is called before the first frame update
    void Start()
    {

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
