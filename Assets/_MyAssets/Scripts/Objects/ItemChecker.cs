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
    [SerializeField] private GameObject reviewSection;

    //힌트 확인처리
    private bool[] hints = new bool[30];    //힌트를 열람한 적이 있는지 기록

    [SerializeField] private Text startButtonText;
    [SerializeField] private Image progessBarFill;
    [SerializeField] private Text chekingText;
    [SerializeField] private Button continueButton;

    private bool isFirstHint = true;
    private bool isFirstBomb = true;

    //진짜 힌트 화면
    [SerializeField] private Text hintTitle;
    [SerializeField] private Text hintText;
    [SerializeField] private Text hintHeader;
    [SerializeField] private GameObject returnToMainButtons;
    [SerializeField] private GameObject returnToReviewButtons;

    //꽝 화면
    [SerializeField] private Sprite[] bombSprites;
    [SerializeField] private Image bombImage;
    [SerializeField] private Text bombHeader;

    //힌트 다시보기 화면
    [SerializeField] private Text[] reviewHintIndexTexts;

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
            player.shouldMoveFreeze = true;
            player.shouldCameraFreeze = true;
            player.shouldDamageFreeze = true;
            inGameUIController.CloseAllCanvas();
            USBCheckCanvas.gameObject.SetActive(true);
            player.doingTask = true;
            itemText.text = "";
            startButtonText.text = "소지중인 USB 확인하기";
        }
    }

    public void CheckUSB()
    {
        if (!itemHolder.HasUSBItem)
        {
            startButtonText.text = "USB 없음";
            return;
        }
        itemHolder.UseUSBItem();
        StartCoroutine(ShowChecking());
    }

    IEnumerator ShowChecking()
    {
        mainSection.SetActive(false);
        checkSection.SetActive(true);

        float fillAmount = 0f;
        chekingText.text = "USB 읽는 중...";
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
        } while (hints[rand]);
        hints[rand] = true;

        Debug.Assert(rand >= 0 && rand < 30);
        ShowHint(rand, true);
    }

    public void ShowHint(int idx, bool shouldBackToMain)
    {
        checkSection.SetActive(false);

        //힌트 획득
        if (idx < 10)
        {
            hintSection.SetActive(true);
            switch (idx)
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
            hintHeader.text = "힌트_" + (idx + 1).ToString() + "번.exe";

            if (shouldBackToMain)
            {
                returnToMainButtons.SetActive(true);
                returnToReviewButtons.SetActive(false);
            }
            else
            {
                returnToMainButtons.SetActive(false);
                returnToReviewButtons.SetActive(true);
            }
        }
        else
        {
            bombSection.SetActive(true);
            bombImage.sprite = bombSprites[idx - 10];
            bombHeader.text = "꽝_" + (idx + 1 - 10).ToString() + "번.exe";
        }
    }

    public void FinishCheckingAndReturnToMain()
    {
        hintSection.SetActive(false);
        bombSection.SetActive(false);
        reviewSection.SetActive(false);
        mainSection.SetActive(true);
    }

    public void FinishCheckingAndReturnToReview()
    {
        hintSection.SetActive(false);
        bombSection.SetActive(false);
        reviewSection.SetActive(true);
    }

    public void SelectHints()
    {
        for(int i = 0; i < 10; i++)
        {
            if(!hints[i])
            {
                reviewHintIndexTexts[i].text = "?";
            }
        }
        mainSection.SetActive(false);
        reviewSection.SetActive(true);
    }

    public void ReviewHints(int idx)
    {
        if (!hints[idx])
        {
            return;
        }
        reviewSection.SetActive(false);
        checkSection.SetActive(true);
        ShowHint(idx, false);
    }

    public void InputPassword()
    {

    }

    public void Exit()
    {
        Time.timeScale = 1f;
        player.shouldMoveFreeze = false;
        player.shouldCameraFreeze = false;
        player.shouldDamageFreeze = false;
        USBCheckCanvas.gameObject.SetActive(false);
        player.doingTask = false;
        player.SetCursorLockState(CursorLockMode.Locked);
        itemText.text = "F키를 눌러 컴퓨터 사용";
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
