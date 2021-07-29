using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditSceneController : MonoBehaviour
{
    [SerializeField] UIElementFade uiElementFade;
    [SerializeField] Button backToLobbySceneButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeCover()
    {
        uiElementFade.CallFadeCoroutine();
    }

    public void FinishCredit()
    {
        backToLobbySceneButton.gameObject.SetActive(false);
        StartCoroutine(MoveToLobby());
    }

    IEnumerator MoveToLobby()
    {

        yield return new WaitForSeconds(1.0f);
        uiElementFade.LoadSceneAfterBlackout("1.0, Lobby");
    }
}
