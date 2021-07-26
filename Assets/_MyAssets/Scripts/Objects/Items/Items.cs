using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Items : MonoBehaviour
{
    public int itemIndex;

    [SerializeField] private EFieldItemType type;
    ItemHolder itemHolder;
    private Text itemText;
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
            switch(type)
            {
                case EFieldItemType.USB:
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
                    break;
                case EFieldItemType.Coin:
                    itemHolder.IncreaseCoin();
                    Destroy(gameObject);
                    itemText.text = "";
                    Debug.Log("���� ȹ��");
                    break;
                default:
                    Debug.Assert(false);
                    break;
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
