using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VendingMachine : MonoBehaviour
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
            if (itemHolder.Coin > 0)
            {
                itemText.text = "������ �̱� �Ϸ�(���� �ʿ�)";
                Debug.Log("������ �̱⸦ ��������");
                itemHolder.DecreaseCoin();
                itemHolder.GetHealthItem();
            }
            else
            {
                itemText.text = "������ �����մϴ�.";
                Debug.Log("���� ����");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isActivated = true;
            if (itemHolder.Coin > 0)
            {
                itemText.text = "FŰ�� ���� ������ �̱�";
            }
            else
            {
                itemText.text = "���� ȹ�� �ʿ�";
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
