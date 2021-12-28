using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemChecker : MonoBehaviour
{
    private PlayerMovement player;
    private ItemHolder itemHolder;
    private Text itemText;
    [SerializeField] private CheckButtonPressed interactButton;
    private bool isActivated = false;

    [SerializeField] private InGameUIController inGameUIController;
    [SerializeField] private Canvas USBCheckCanvas;

    //창 이동 처리
    [SerializeField] private GameObject mainSection;
    [SerializeField] private GameObject checkSection;
    [SerializeField] private GameObject hintSection;
    [SerializeField] private GameObject bombSection;
    [SerializeField] private GameObject reviewSection;
    [SerializeField] private GameObject tutorialSection;
    [SerializeField] private GameObject passwordSection;

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
    [SerializeField] private GameObject returnToMainButtons1;
    [SerializeField] private GameObject returnToReviewButtons;
    [SerializeField] private GameObject returnToHintTutorialButtons;

    //꽝 화면
    [SerializeField] private Sprite[] bombSprites;
    [SerializeField] private Image bombImage;
    [SerializeField] private Text bombHeader;
    [SerializeField] private GameObject returnToMainButtons2;
    [SerializeField] private GameObject returnToBombTutorialButtons;

    //힌트 다시보기 화면
    [SerializeField] private Text[] reviewHintIndexTexts;

    //튜토리얼 화면
    [SerializeField] private Text tutorialTitle;
    [SerializeField] private Text tutorialDescription;

    //암호입력 화면
    [SerializeField] private InputField passwordInputField;
    [SerializeField] private Text passwordErrorText;
    [SerializeField] private SceneResetManager sceneResetManager;

    // Start is called before the first frame update
    void Start()
    {
        passwordInputField.onValueChanged.AddListener(delegate { ClearPasswordError(); });
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
#if UNITY_STANDALONE_WIN
        bool keyDown = Input.GetKeyDown(KeyCode.F);
#elif UNITY_ANDROID || UNITY_IOS
        bool keyDown = interactButton.pressed;
#endif
        if (isActivated && !player.isStunned && !player.isDead && !player.doingTask && keyDown)
        {
#if UNITY_ANDROID || UNITY_IOS
            interactButton.pressed = false;
            interactButton.gameObject.SetActive(false);
#endif
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
        Debug.Log("힌트뽑기: " + rand.ToString());
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

            if (isFirstHint)
            {
                returnToHintTutorialButtons.SetActive(true);
                returnToMainButtons1.SetActive(false);
                returnToReviewButtons.SetActive(false);
            }
            else if (shouldBackToMain)
            {
                returnToHintTutorialButtons.SetActive(false);
                returnToMainButtons1.SetActive(true);
                returnToReviewButtons.SetActive(false);
            }
            else
            {
                returnToHintTutorialButtons.SetActive(false);
                returnToMainButtons1.SetActive(false);
                returnToReviewButtons.SetActive(true);
            }
        }
        else
        {
            if (isFirstBomb)
            {
                returnToBombTutorialButtons.SetActive(true);
                returnToMainButtons2.SetActive(false);
            }
            else
            {
                returnToBombTutorialButtons.SetActive(false);
                returnToMainButtons2.SetActive(true);
            }

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
        tutorialSection.SetActive(false);
        passwordSection.SetActive(false);
        mainSection.SetActive(true);
    }

    public void FinishCheckingAndReturnToReview()
    {
        hintSection.SetActive(false);
        bombSection.SetActive(false);
        reviewSection.SetActive(true);
    }

    public void ReturnToTutorial(bool b)
    {
        hintSection.SetActive(false);
        bombSection.SetActive(false);
        tutorialSection.SetActive(true);
        Tutorial(b);
    }

    public void SelectHints()
    {
        mainSection.SetActive(false);
        reviewSection.SetActive(true);
        for (int i = 0; i < 10; i++)
        {
            Debug.Log("힌트체크: " + i.ToString() + " - " + hints[i].ToString());
            if (!hints[i])
            {
                reviewHintIndexTexts[i].text = "?";
            }
            else
            {
                reviewHintIndexTexts[i].text = "#" + (i + 1).ToString();
            }
        }
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
        passwordSection.SetActive(true);
        passwordErrorText.gameObject.SetActive(false);
        passwordInputField.text = "";
    }

    public void ConfirmPassword()
    {
        if(passwordInputField.text == "76051")
        {
            sceneResetManager.ClearAllObjectsAndLoadScene("CorrectPassword");
        }
        else
        {
            passwordInputField.text = "";
            passwordErrorText.gameObject.SetActive(true);

        }
    }

    void ClearPasswordError()
    {
        Debug.Log("삭제");

        passwordErrorText.gameObject.SetActive(false);
    }

    public void Tutorial(bool isBomb)
    {
        tutorialSection.SetActive(true);

        if (isBomb)
        {
            tutorialTitle.text = "이런!";
            tutorialDescription.text = "전혀 도움이 되지 않는 USB였습니다." +
                " 앞으로도 이런 USB가 여러개 섞여있을 것입니다." +
                " 또 얼마나 짜증나는 이미지가 들어있을까요?";
            isFirstBomb = false;
        }
        else
        {
            tutorialTitle.text = "축하합니다!";
            tutorialDescription.text = "도움이 될 만한 힌트를 얻었습니다." +
                " 이러한 힌트들을 잘 읽고 비밀번호를 입력합시다." +
                " 그러면 디버깅용 코드가 실행되고 버그가 만연한 이 곳에서 탈출할 수 있습니다!";
            isFirstHint = false;
        }

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
#if UNITY_STANDALONE_WIN
            itemText.text = "F키를 눌러 컴퓨터 사용";
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
            itemText.text = "";
        }
    }
}
