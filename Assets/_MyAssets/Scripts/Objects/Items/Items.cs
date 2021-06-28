using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    ItemHolder itemHolder;

    public EItemType type;

    // Start is called before the first frame update
    void Start()
    {
        itemHolder = ItemHolder.Instance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            switch (type)
            {
                case EItemType.USB:
                    if (itemHolder.TryGetUSB())
                    {
                        Destroy(gameObject);
                        Debug.Log("USB ȹ��");
                    }
                    else
                    {
                        Debug.Log("�̹� USB ������");
                    }
                    break;
                case EItemType.SpeedUp:
                    Destroy(gameObject);
                    Debug.Log("������ ȹ��� ���ÿ� ȿ�� ����");
                    break;
            }

        }
    }
}
