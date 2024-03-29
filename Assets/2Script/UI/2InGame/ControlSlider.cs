using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlSlider : MonoBehaviour
{
    [SerializeField] private Image fillImage = null;
    [SerializeField] private Text remainText = null;

    private int maxValue = 0;
    private int curValue = 0;

    public void SetMaxValue(int p_value)
    {
        maxValue = p_value;
        fillImage.fillAmount = 1f;

        if (!(remainText is null)) remainText.text = maxValue.ToString();
    }

    public void SetValue(int p_value)
    {
        curValue = p_value;
        fillImage.fillAmount = (float)curValue / maxValue;

        if (!(remainText is null)) remainText.text = curValue.ToString();
    }
}
