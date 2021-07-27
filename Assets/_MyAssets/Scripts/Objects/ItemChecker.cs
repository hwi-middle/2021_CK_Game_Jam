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

    //창 이동 처리
    [SerializeField] private GameObject mainSection;
    [SerializeField] private GameObject checkSection;

    //힌트 확인처리
    [SerializeField] private Sprite[] hintSprites;
    private bool[] hints = new bool[30];    //힌트를 열람한 적이 있는지 기록

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
        //        itemText.text = "USB 판독 완료(구현 필요)";
        //        Debug.Log("USB 판독을 진행했음");
        //        itemHolder.UseUSBItem();
        //    }
        //    else
        //    {
        //        Debug.Log("판독할 USB가 없음");
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

        chekingText.text = "파일 읽기 완료";
        Debug.Log("힌트 뽑기 결과: " + rand.ToString());
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
            itemText.text = "F키를 눌러 컴퓨터 사용";
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
