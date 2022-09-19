using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUIManager : MonoBehaviour
{
    [SerializeField] private TitleManager titleManager = null;
    [SerializeField] private Image titleImage = null;
    [SerializeField] private Button gameStartButton = null;

    public void OnClickGameStartButton() => StartCoroutine(StartEffect());

    private void Start()
    {
        UIManager.DisableJoystick();
    }

    private IEnumerator StartEffect()
    {
        gameStartButton.interactable = false;

        titleImage.GetComponent<Animator>().SetTrigger("On");
        gameStartButton.GetComponent<Animator>().SetTrigger("On");

        yield return new WaitForSeconds(1f);

        titleManager.GameStart();
        UIManager.ActivateJoystick();
    }
}
