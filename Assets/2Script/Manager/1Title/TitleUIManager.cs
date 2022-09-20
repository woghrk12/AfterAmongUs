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

    private void Start()
    {
        UIManager.DisablePlayerUI();
        UIManager.FadeIn();
    }

    public void OnClickGameStartButton() => StartCoroutine(StartEffect());

    #region OnOffFunction
    public void OnCustomUI() => customUI.SetActive(true);
    public void OffCustomUI() => customUI.SetActive(false);
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
