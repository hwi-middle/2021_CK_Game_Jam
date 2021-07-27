using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    private static ItemHolder instance;
    public static ItemHolder Instance { get { Init(); return instance; } }

    private bool hasUSBItem = false;
    public bool HasUSBItem { get { return hasUSBItem; } }

    private int USBIndex = 0;
    public int USBItemIndex { get { return USBIndex; } }

    private int coin = 0;
    public int Coin { get { return coin; } }

    private bool hasHealthItem = false;
    public bool HasHealthItem { get { return hasHealthItem; } set { hasHealthItem = value; } }

    private EItemType type;
    public EItemType HealthItemType { get { return type; } }

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

    public void GetUSBItem(int itemIdx)
    {
        hasUSBItem = true;
        USBIndex = itemIdx;
    }

    public void UseUSBItem()
    {
        hasUSBItem = false;
        USBIndex = 0;
    }

    public void GetHealthItem()
    {
        int rand = Random.Range(0, 3);
        type = (EItemType)rand;
        hasHealthItem = true;
    }

    public void IncreaseCoin()
    {
        coin++;
    }

    public void DecreaseCoin()
    {
        if (coin <= 0) return;
        coin--;
    }
}
