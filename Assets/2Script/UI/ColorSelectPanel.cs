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
        cancelImage.SetActive(true);
        background.SetActive(true);
        animSelectPanel.SetTrigger("On");
    }

    public void DisablePanel()
    {
        cancelImage.SetActive(false);
        background.SetActive(false);
        animSelectPanel.SetTrigger("Off");
    }
}
