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

    private bool inProgress = false;

    private void Start()
    {
        UIManager.DisablePlayerUI();
        UIManager.FadeIn();
    }

    public void OnClickGameStartButton() => StartCoroutine(StartEffect());

    #region OnOffFunction

    public void OnCustomUI()
    {
        if (inProgress) return;
        UIManager.DisablePlayerUI();
        StartCoroutine(OpenUI(customUI.GetComponent<Animator>(), 0.2f));
    }
    public void OffCustomUI()
    {
        if (inProgress) return;
        UIManager.ActivatePlayerUI();
        StartCoroutine(CloseUI(customUI.GetComponent<Animator>(), 0.2f));
    }
    private IEnumerator CloseUI(Animator p_anim, float p_time)
    {
        if (p_anim == null) yield break;
        if (!p_anim.gameObject.activeSelf) yield break;

        inProgress = true;

        p_anim.SetTrigger("Off");
        yield return new WaitForSeconds(p_time);
        p_anim.gameObject.SetActive(false);

        inProgress = false;
    }
    private IEnumerator OpenUI(Animator p_anim, float p_time)
    {
        if (p_anim == null) yield break;

        inProgress = true;

        if (!p_anim.gameObject.activeSelf) p_anim.gameObject.SetActive(true);
        p_anim.SetTrigger("On");
        yield return new WaitForSeconds(p_time);

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
    }
}
