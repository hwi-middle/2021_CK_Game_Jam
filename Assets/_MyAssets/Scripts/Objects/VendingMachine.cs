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
                itemText.text = "������ �����մϴ�.";
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
                itemEffect.text = "ü�� +50%";
                itemDescription.text = "���� ��������! �������� �÷��ִ� ������ �ʼ� �������Դϴ�.";
                break;
            case EItemType.TomatoJuice:
                itemImage.sprite = itemSprites[1];
                itemName.text = "�丶����";
                itemEffect.text = "ü�� +25%";
                itemDescription.text = "������ �丶�� ���Դϴ�. �ֽ����ٴ� ��.. �� ��������.";
                break;
            case EItemType.CucumberJuice:
                itemImage.sprite = itemSprites[2];
                itemName.text = "�����ֽ�";
                itemEffect.text = "ü�� +15%";
                itemDescription.text = "����, �̰� ���� ������? ������ ���� �� �������� ���� �����ϱ���.";
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
            RetryButton.transform.GetChild(0).GetComponent<Text>().text = "���κ���";
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
