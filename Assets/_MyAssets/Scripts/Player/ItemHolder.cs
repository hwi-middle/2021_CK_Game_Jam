using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    static ItemHolder instance;
    public static ItemHolder Instance { get { Init(); return instance; } }

    bool hasUSBItem = false;
    public bool HasUSBItem { get { return hasUSBItem; } set { hasUSBItem = value; } }

    private void Awake()
    {
        Init();
    }

    static void Init()
    {
        if (instance == null)
        {
            GameObject go = GameObject.FindWithTag("Player");
            if (go == null)
            {
                Debug.LogError("Player not found");
            }

            instance = go.GetComponent<ItemHolder>();
        }
    }

    //���Ǳ� �����ۿ��� ����� �Լ�, �����ʿ�
    public void GetHealthItem(int amount)
    {
        return;
    }
}
