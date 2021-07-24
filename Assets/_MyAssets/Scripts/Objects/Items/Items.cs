using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Items : MonoBehaviour
{
    public int itemIndex;

    ItemHolder itemHolder;
    [SerializeField] private Text itemText;
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
        if(isActivated && Input.GetKeyDown(KeyCode.F))
        {
            if (itemHolder.HasUSBItem)
            {
                Debug.Log("�̹� USB ������");
            }
            else
            {
                itemHolder.GetUSBItem(itemIndex);
                Destroy(gameObject);
                itemText.text = "";
                Debug.Log("USB ȹ��");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isActivated = true;
            itemText.text = "FŰ�� ���� ������ ȹ��";
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
