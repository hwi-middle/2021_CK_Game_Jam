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
                itemText.text = "아이템 뽑기 완료(구현 필요)";
                Debug.Log("아이템 뽑기를 진행했음");
                itemHolder.DecreaseCoin();
                itemHolder.GetHealthItem();
            }
            else
            {
                itemText.text = "코인이 부족합니다.";
                Debug.Log("코인 부족");
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
                itemText.text = "F키를 눌러 아이템 뽑기";
            }
            else
            {
                itemText.text = "코인 획득 필요";
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
