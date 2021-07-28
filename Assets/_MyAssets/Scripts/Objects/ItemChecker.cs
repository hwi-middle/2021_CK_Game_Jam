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
    [SerializeField] private GameObject hintSection;
    [SerializeField] private GameObject bombSection;
    [SerializeField] private GameObject reviewSection;
    [SerializeField] private GameObject tutorialSection;
    [SerializeField] private GameObject passwordSection;

    //��Ʈ Ȯ��ó��
    private bool[] hints = new bool[30];    //��Ʈ�� ������ ���� �ִ��� ���

    [SerializeField] private Text startButtonText;
    [SerializeField] private Image progessBarFill;
    [SerializeField] private Text chekingText;
    [SerializeField] private Button continueButton;

    private bool isFirstHint = true;
    private bool isFirstBomb = true;

    //��¥ ��Ʈ ȭ��
    [SerializeField] private Text hintTitle;
    [SerializeField] private Text hintText;
    [SerializeField] private Text hintHeader;
    [SerializeField] private GameObject returnToMainButtons1;
    [SerializeField] private GameObject returnToReviewButtons;
    [SerializeField] private GameObject returnToHintTutorialButtons;

    //�� ȭ��
    [SerializeField] private Sprite[] bombSprites;
    [SerializeField] private Image bombImage;
    [SerializeField] private Text bombHeader;
    [SerializeField] private GameObject returnToMainButtons2;
    [SerializeField] private GameObject returnToBombTutorialButtons;

    //��Ʈ �ٽú��� ȭ��
    [SerializeField] private Text[] reviewHintIndexTexts;

    //Ʃ�丮�� ȭ��
    [SerializeField] private Text tutorialTitle;
    [SerializeField] private Text tutorialDescription;

    //��ȣ�Է� ȭ��
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
            startButtonText.text = "�������� USB Ȯ���ϱ�";
        }
    }

    public void CheckUSB()
    {
        if (!itemHolder.HasUSBItem)
        {
            startButtonText.text = "USB ����";
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
        chekingText.text = "USB �д� ��...";
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

        chekingText.text = "USB �б� �Ϸ�";
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

        //��Ʈ ȹ��
        if (idx < 10)
        {
            hintSection.SetActive(true);
            switch (idx)
            {
                case 0:
                    hintTitle.text = "ù";
                    hintText.text = "7";
                    break;
                case 1:
                    hintTitle.text = "��";
                    hintText.text = "6";
                    break;
                case 2:
                    hintTitle.text = "��";
                    hintText.text = "0";
                    break;
                case 3:
                    hintTitle.text = "��";
                    hintText.text = "5";
                    break;
                case 4:
                    hintTitle.text = "�ټ�";
                    hintText.text = "1";
                    break;
                case 5:
                    hintTitle.text = "ù";
                    hintText.text = "����� ����";
                    break;
                case 6:
                    hintTitle.text = "��";
                    hintText.text = "�Ǹ��� ����";
                    break;
                case 7:
                    hintTitle.text = "��";
                    hintText.text = "������ ����";
                    break;
                case 8:
                    hintTitle.text = "��";
                    hintText.text = "�ϼ��� ����";
                    break;
                case 9:
                    hintTitle.text = "�ټ�";
                    hintText.text = "������ ����";
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
            hintTitle.text += " ��° �ڸ��� ...";
            hintHeader.text = "��Ʈ_" + (idx + 1).ToString() + "��.exe";

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
            bombHeader.text = "��_" + (idx + 1 - 10).ToString() + "��.exe";
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
        for (int i = 0; i < 10; i++)
        {
            if (!hints[i])
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
        Debug.Log("����");

        passwordErrorText.gameObject.SetActive(false);
    }

    public void Tutorial(bool isBomb)
    {
        tutorialSection.SetActive(true);

        if (isBomb)
        {
            tutorialTitle.text = "�̷�!";
            tutorialDescription.text = "���� ������ ���� �ʴ� USB�����ϴ�." +
                " �����ε� �̷� USB�� ������ �������� ���Դϴ�." +
                " �� �󸶳� ¥������ �̹����� ����������?";
            isFirstBomb = false;
        }
        else
        {
            tutorialTitle.text = "�����մϴ�!";
            tutorialDescription.text = "������ �� ���� ��Ʈ�� ������ϴ�." +
                " �̷��� ��Ʈ���� �� �а� ��й�ȣ�� �Է��սô�." +
                " �׷��� ������ �ڵ尡 ����ǰ� ���װ� ������ �� ������ Ż���� �� �ֽ��ϴ�!";
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
        itemText.text = "FŰ�� ���� ��ǻ�� ���";
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
