using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Items : MonoBehaviour
{
    public int itemIndex;

    [SerializeField] private EFieldItemType type;
    private InGameUIController inGameUIController;
    private ItemHolder itemHolder;
    public CheckButtonPressed interactButton;
    private Text itemText;
    bool isActivated = false;

    // Start is called before the first frame update
    void Start()
    {
        itemHolder = ItemHolder.Instance;
        itemText = GameObject.FindWithTag("ItemText").GetComponent<Text>();
        inGameUIController = PlayerMovement.Instance.GetComponent<InGameUIController>();
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_STANDALONE_WIN
        bool keyDown = Input.GetKeyDown(KeyCode.F);
#elif UNITY_ANDROID || UNITY_IOS
        bool keyDown = interactButton.pressed;
#endif
        if (isActivated && keyDown)
        {
#if UNITY_ANDROID || UNITY_IOS
            interactButton.pressed = false;
            interactButton.gameObject.SetActive(false);
#endif
            switch (type)
            {
                case EFieldItemType.USB:
                    if (itemHolder.HasUSBItem)
                    {
                        Debug.Log("이미 USB 소유함");
#if UNITY_ANDROID || UNITY_IOS
                        interactButton.gameObject.SetActive(true);
#endif
                    }
                    else
                    {
                        itemHolder.GetUSBItem(itemIndex);
                        Destroy(gameObject);
                        itemText.text = "";
                        Debug.Log("USB 획득");
                    }

                    break;
                case EFieldItemType.Coin:
                    itemHolder.IncreaseCoin();
                    Destroy(gameObject);
                    itemText.text = "";
                    Debug.Log("코인 획득");
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
#if UNITY_STANDALONE_WIN
            itemText.text = "F키를 눌러 아이템 획득";
#elif UNITY_ANDROID || UNITY_IOS
            interactButton.gameObject.SetActive(true);
#endif
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isActivated = false;
#if UNITY_STANDALONE_WIN
            itemText.text = "";
#elif UNITY_ANDROID || UNITY_IOS
            interactButton.gameObject.SetActive(false);
#endif
        }
    }
}