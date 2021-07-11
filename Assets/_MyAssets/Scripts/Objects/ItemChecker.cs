using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemChecker : MonoBehaviour
{
    ItemHolder itemHolder;
    Text itemText;
    bool isActivated = false;

    // Start is called before the first frame update
    void Start()
    {
        itemHolder = ItemHolder.Instance;
        itemText = GameObject.FindWithTag("ItemText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivated && Input.GetKeyDown(KeyCode.F))
        {
            if (itemHolder.HasUSBItem)
            {
                itemText.text = "USB 판독 완료(구현 필요)";
                Debug.Log("USB 판독을 진행했음");
                itemHolder.UseItem();
            }
            else
            {
                Debug.Log("판독할 USB가 없음");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isActivated = true;
            if (itemHolder.HasUSBItem)
            {
                itemText.text = "F키를 눌러 USB 판독 시작";
            }
            else
            {
                itemText.text = "USB 획득 필요";
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isActivated = false;
            itemText.text = "";
        }
    }
}
