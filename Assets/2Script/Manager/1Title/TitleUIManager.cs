using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUIManager : MonoBehaviour
{
    [SerializeField] private TitleManager titleManager = null;
    [SerializeField] private Image titleImage = null;
    [SerializeField] private Button gameStartButton = null;
    [SerializeField] private GameObject customUI = null;
    [SerializeField] private GameObject shipUI = null;

    private bool inProgress = false;

    private void Start()
    {
        UIManager.DisablePlayerUI();
        UIManager.FadeIn();
    }

    public void OnClickGameStartButton() => StartCoroutine(StartEffect());
    public void OnClickInGameButton()
    {
        if (!titleManager.CheckCanStartGame()) return;

        OffShipUI();
        titleManager.EnterInGame();
        UIManager.DisablePlayerUI();
        UIManager.FadeOut();
    }

    #region OnOffFunction

    public void OnCustomUI()
    {
        if (inProgress) return;
        UIManager.DisablePlayerUI();
        OffShipUI();
        StartCoroutine(OpenUI(customUI, 0.2f));
    }
    public void OffCustomUI()
    {
        if (inProgress) return;
        UIManager.ActivatePlayerUI();
        OnShipUI();
        StartCoroutine(CloseUI(customUI, 0.2f));
    }
    public void OnShipUI() => StartCoroutine(OpenUI(shipUI));
    public void OffShipUI() => StartCoroutine(CloseUI(shipUI));

    private IEnumerator CloseUI(GameObject p_uiObj, float p_time = 0f)
    {
        if (!p_uiObj.activeSelf) yield break;
        
        inProgress = true;

        var t_anim = p_uiObj.GetComponent<Animator>();
        if (t_anim != null)
        {
            t_anim.SetTrigger("Off");
            yield return new WaitForSeconds(p_time);
        }
        
        p_uiObj.SetActive(false);

        inProgress = false;
    }
    private IEnumerator OpenUI(GameObject p_uiObj, float p_time = 0f)
    {
        if (p_uiObj.activeSelf) yield break;

        inProgress = true;

        p_uiObj.SetActive(true);

        var t_anim = p_uiObj.GetComponent<Animator>();
        if (t_anim != null)
        {
            t_anim.SetTrigger("On");
            yield return new WaitForSeconds(p_time);
        }
    
        inProgress = false;
    }

    #endregion

    private IEnumerator StartEffect()
    {
        gameStartButton.interactable = false;

        titleImage.GetComponent<Animator>().SetTrigger("On");
        gameStartButton.GetComponent<Animator>().SetTrigger("On");

        yield return new WaitForSeconds(1f);

        titleManager.GameStart();
        UIManager.ActivatePlayerUI();
        OnShipUI();
    }
}
