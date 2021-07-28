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
    [SerializeField] private GameObject hintSection;
    [SerializeField] private GameObject bombSection;

    //힌트 확인처리
    private bool[] hints = new bool[30];    //힌트를 열람한 적이 있는지 기록

    [SerializeField] private Image progessBarFill;
    [SerializeField] private Text chekingText;
    [SerializeField] private Button continueButton;

    private bool isFirstHint = true;
    private bool isFirstBomb = true;

    //진짜 힌트 화면
    [SerializeField] private Text hintTitle;
    [SerializeField] private Text hintText;
    [SerializeField] private Text hintHeader;

    //꽝 화면
    [SerializeField] private Sprite[] bombSprites;
    [SerializeField] private Image bombImage;
    [SerializeField] private Text bombHeader;

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
        mainSection.SetActive(false);
        checkSection.SetActive(true);

        float fillAmount = 0f;
        progessBarFill.fillAmount = 0f;
        continueButton.gameObject.SetActive(false);

        while (progessBarFill.fillAmount != 1)
        {
            fillAmount += 0.5f * Time.deltaTime;
            if (fillAmount > 1) fillAmount = 1;
            progessBarFill.fillAmount = fillAmount;
            yield return null;
        }
        yield return new WaitForSecondsRealtime(0.5f);

        chekingText.text = "USB 읽기 완료";
        continueButton.gameObject.SetActive(true);
    }

    public void CompleteChecking()
    {
        int rand;
        do
        {
            rand = Random.Range(0, 30);
            Debug.Log(hints[rand]);
        } while (hints[rand]);
        hints[rand] = true;

        Debug.Assert(rand >= 0 && rand < 30);
        checkSection.SetActive(false);

        //힌트 획득
        if (rand < 10)
        {
            hintSection.SetActive(true);
            switch (rand)
            {
                case 0:
                    hintTitle.text = "첫";
                    hintText.text = "7";
                    break;
                case 1:
                    hintTitle.text = "두";
                    hintText.text = "6";
                    break;
                case 2:
                    hintTitle.text = "세";
                    hintText.text = "0";
                    break;
                case 3:
                    hintTitle.text = "네";
                    hintText.text = "5";
                    break;
                case 4:
                    hintTitle.text = "다섯";
                    hintText.text = "1";
                    break;
                case 5:
                    hintTitle.text = "첫";
                    hintText.text = "행운의 숫자";
                    break;
                case 6:
                    hintTitle.text = "두";
                    hintText.text = "악마의 숫자";
                    break;
                case 7:
                    hintTitle.text = "세";
                    hintText.text = "공허의 숫자";
                    break;
                case 8:
                    hintTitle.text = "네";
                    hintText.text = "완성의 숫자";
                    break;
                case 9:
                    hintTitle.text = "다섯";
                    hintText.text = "태초의 시작";
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
            hintTitle.text += " 번째 자리는 ...";
            hintHeader.text = "힌트_" + rand.ToString() + ".exe";
        }
        else
        {
            bombSection.SetActive(true);
            bombImage.sprite = bombSprites[rand - 10];
            bombHeader.text = "꽝_" + (rand - 10).ToString() + ".exe";
        }

    }

    public void FinishChecking()
    {
        hintSection.SetActive(false);
        bombSection.SetActive(false);
        mainSection.SetActive(true);
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
