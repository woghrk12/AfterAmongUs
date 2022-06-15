using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSelectPanel : MonoBehaviour
{
    [SerializeField] private GameObject selectPanel;
    private Animator anim;

    private void Awake()
    {
        anim = selectPanel.GetComponent<Animator>();
    }

    public void EnablePanel()
    {
        anim.SetTrigger("On");
    }

    public void DisablePanel()
    {
        anim.SetTrigger("Off");
    }
}
