using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemChecker : MonoBehaviour
{
    private PlayerMovement player;
    private ItemHolder itemHolder;
    private Text itemText;
    private bool isActivated = false;

    [SerializeField] private InGameUIController inGameUIController;
    [SerializeField] private Canvas USBCheckCanvas;

    //â �̵� ó��
    [SerializeField] private GameObject mainSection;
    [SerializeField] private GameObject checkSection;

    //��Ʈ Ȯ��ó��
    [SerializeField] private Sprite[] hintSprites;
    private bool[] hints = new bool[30];    //��Ʈ�� ������ ���� �ִ��� ���

    [SerializeField] private Image progessBarFill;
    [SerializeField] private Text chekingText;
    [SerializeField] private Button continueButton;

    private bool isFirstHint = true;
    private bool isFirstBomb = true;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 30; i++)
        {
            hints[i] = false;
        }
        player = PlayerMovement.Instance;
        itemHolder = ItemHolder.Instance;
        itemText = GameObject.FindWithTag("ItemText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivated && !player.isStunned && !player.isDead && !player.doingTask && Input.GetKeyDown(KeyCode.F))
        {
            player.SetCursorLockState(CursorLockMode.None);
            //Time.timeScale = 0f;
            player.shouldCameraFreeze = true;
            inGameUIController.CloseAllCanvas();
            USBCheckCanvas.gameObject.SetActive(true);
            player.doingTask = true;
            itemText.text = "";
        }

        //if (isActivated && Input.GetKeyDown(KeyCode.F))
        //{
        //    if (itemHolder.HasUSBItem)
        //    {
        //        itemText.text = "USB �ǵ� �Ϸ�(���� �ʿ�)";
        //        Debug.Log("USB �ǵ��� ��������");
        //        itemHolder.UseUSBItem();
        //    }
        //    else
        //    {
        //        Debug.Log("�ǵ��� USB�� ����");
        //    }
        //}
    }

    public void CheckUSB()
    {
        StartCoroutine(ShowChecking());
    }

    IEnumerator ShowChecking()
    {
        int rand;
        do
        {
            rand = Random.Range(0, 30);
        } while (hints[rand]);

        mainSection.SetActive(false);
        checkSection.SetActive(true);

        float fillAmount = 0f;
        while (progessBarFill.fillAmount != 1)
        {
            fillAmount += 0.5f * Time.deltaTime;
            if (fillAmount > 1) fillAmount = 1;
            progessBarFill.fillAmount = fillAmount;
            yield return null;
        }
        yield return new WaitForSecondsRealtime(0.5f);

        chekingText.text = "���� �б� �Ϸ�";
        Debug.Log("��Ʈ �̱� ���: " + rand.ToString());
        continueButton.gameObject.SetActive(true);
    }

    public void CompleteChecking(int idx)
    {
        Debug.Assert(idx >= 0 && idx < 30);
        if(idx < 5)
        {

        }
        else if (idx < 10)
        {

        }
        else
        {

        }
    }

    public void ReviewHints()
    {

    }

    public void InputPassword()
    {

    }

    public void Exit()
    {
        Time.timeScale = 1f;
        player.shouldCameraFreeze = false;
        USBCheckCanvas.gameObject.SetActive(false);
        player.doingTask = false;
        player.SetCursorLockState(CursorLockMode.Locked);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isActivated = true;
            itemText.text = "FŰ�� ���� ��ǻ�� ���";
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
