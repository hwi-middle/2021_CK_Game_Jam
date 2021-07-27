using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VendingMachine : MonoBehaviour
{
    private PlayerMovement player;
    private ItemHolder itemHolder;
    private Text itemText;
    private bool isActivated = false;

    [SerializeField] private InGameUIController inGameUIController;
    [SerializeField] private Canvas vendingMachineCanvas;
    [SerializeField] private Sprite[] itemSprites;
    [SerializeField] private Image itemImage;
    [SerializeField] private Text itemName;
    [SerializeField] private Text itemEffect;
    [SerializeField] private Text itemDescription;
    [SerializeField] private Button RetryButton;
    [SerializeField] private Button ExitButton;
    [SerializeField] private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerMovement.Instance;
        itemHolder = ItemHolder.Instance;
        itemText = GameObject.FindWithTag("ItemText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivated && !player.isStunned && !player.isDead && !player.doingTask && Input.GetKeyDown(KeyCode.F))
        {
            if (itemHolder.Coin > 0)
            {
                player.SetCursorLockState(CursorLockMode.None);
                Time.timeScale = 0f;
                player.shouldCameraFreeze = true;
                inGameUIController.CloseAllCanvas();
                vendingMachineCanvas.gameObject.SetActive(true);
                player.doingTask = true;
                itemText.text = "";

                itemHolder.DecreaseCoin();
                StartCoroutine(ShowGacha());
            }
            else
            {
                itemText.text = "코인이 부족합니다.";
            }
        }
    }

    IEnumerator ShowGacha()
    {
        int prev = -1;
        for (int i = 0; i < 10; i++)
        {
            int rand;
            do
            {
                rand = Random.Range(0, 3);
            } while (prev == rand);
            prev = rand;
            itemImage.sprite = itemSprites[rand];
            yield return new WaitForSecondsRealtime(0.05f);
        }

        itemHolder.GetHealthItem();
        switch (itemHolder.HealthItemType)
        {
            case EItemType.MonsterEnergy:
                itemImage.sprite = itemSprites[0];
                itemName.text = "MONSTER";
                itemEffect.text = "체력 +50%";
                itemDescription.text = "정신 차리세요! 에너지를 올려주는 개발자 필수 아이템입니다.";
                break;
            case EItemType.TomatoJuice:
                itemImage.sprite = itemSprites[1];
                itemName.text = "토마토즙";
                itemEffect.text = "체력 +25%";
                itemDescription.text = "달콤한 토마토 즙입니다. 주스보다는 좀.. 덜 달지만요.";
                break;
            case EItemType.CucumberJuice:
                itemImage.sprite = itemSprites[2];
                itemName.text = "오이주스";
                itemEffect.text = "체력 +15%";
                itemDescription.text = "으윽, 이건 무슨 맛이죠? 몸에는 좋은 것 같은지만 맛은 끔찍하군요.";
                break;
        }
        audioSource.Play();
        RetryButton.gameObject.SetActive(true);
        ExitButton.gameObject.SetActive(true);
    }

    public void Retry()
    {
        if(itemHolder.Coin <= 0)
        {
            RetryButton.transform.GetChild(0).GetComponent<Text>().text = "코인부족";
        }
        else
        {
            RetryButton.gameObject.SetActive(false);
            ExitButton.gameObject.SetActive(false);
            itemHolder.DecreaseCoin();
            StartCoroutine(ShowGacha());
        }
    }

    public void Exit()
    {
        Time.timeScale = 1f;
        player.shouldCameraFreeze = false;
        RetryButton.gameObject.SetActive(false);
        ExitButton.gameObject.SetActive(false);
        vendingMachineCanvas.gameObject.SetActive(false);
        player.doingTask = false;
        player.SetCursorLockState(CursorLockMode.Locked);
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
