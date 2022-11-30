using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUIGroup : UIGroup
{
    [SerializeField] private Image progressBar = null;

    public override void InitUI()
    {
        progressBar.fillAmount = 0f;
    }

    public void SetProgress(float p_value)
    {
        progressBar.fillAmount = p_value;
    }
}
