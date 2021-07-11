using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    static ItemHolder instance;
    public static ItemHolder Instance { get { Init(); return instance; } }

    int idx = 0;
    public int ItemIndex { get { return idx; } }

    bool hasUSBItem = false;
    public bool HasUSBItem { get { return hasUSBItem; } }

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

    public void GetItem(int itemIdx)
    {
        hasUSBItem = true;
        idx = itemIdx;
    }

    public void UseItem()
    {
        hasUSBItem = false;
        idx = 0;
    }

    //자판기 아이템에서 사용할 함수, 구현필요
    public void GetHealthItem(int amount)
    {
        return;
    }
}
