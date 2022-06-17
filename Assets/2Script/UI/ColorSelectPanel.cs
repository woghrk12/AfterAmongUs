using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSelectPanel : MonoBehaviour
{
    [SerializeField] private GameObject cancelImage;
    [SerializeField] private GameObject background;
    [SerializeField] private Image previewImage;
    [SerializeField] private GameObject selectPanel;
    private Animator animSelectPanel;

    private void Awake()
    {
        animSelectPanel = selectPanel.GetComponent<Animator>();

        var inst = Instantiate(previewImage.material);
        previewImage.material = inst;
    }

    public void EnablePanel()
    {
        StartCoroutine(EnablePanelCo());
    }

    private IEnumerator EnablePanelCo()
    {
        animSelectPanel.SetTrigger("On");

        yield return new WaitForSeconds(1f);

        cancelImage.SetActive(true);
        background.SetActive(true);
    }

    public void DisablePanel()
    {
        StartCoroutine(DisablePanelCo());
    }

    private IEnumerator DisablePanelCo()
    {
        cancelImage.SetActive(false);
        background.SetActive(false);
        animSelectPanel.SetTrigger("Off");

        yield return new WaitForSeconds(1f);
    }

    public void OnClickColorSelectButton(int p_color)
    {
        previewImage.material.SetColor("_PlayerColor", PlayerColor.GetColor((EPlayerColor)p_color));
        TitleManager.ChangePlayerColor((EPlayerColor)p_color);
    }
}
