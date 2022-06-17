using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSelectPanel : MonoBehaviour
{
    [SerializeField] private GameObject cancelImage;
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject selectPanel;
    private Animator animSelectPanel;

    private void Awake()
    {
        animSelectPanel = selectPanel.GetComponent<Animator>();
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
}
