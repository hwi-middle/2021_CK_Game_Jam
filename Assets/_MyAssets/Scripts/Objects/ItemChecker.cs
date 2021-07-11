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
                itemText.text = "USB �ǵ� �Ϸ�(���� �ʿ�)";
                Debug.Log("USB �ǵ��� ��������");
                itemHolder.UseItem();
            }
            else
            {
                Debug.Log("�ǵ��� USB�� ����");
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
                itemText.text = "FŰ�� ���� USB �ǵ� ����";
            }
            else
            {
                itemText.text = "USB ȹ�� �ʿ�";
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
