using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlStatus : MonoBehaviour
{
    [SerializeField] private Image fillImage = null;
    [SerializeField] private Text remainText = null;

    private int maxValue = 0;
    private int curValue = 0;

    public int MaxValue { set { maxValue = value; } get { return maxValue; } }
    
    public void SetValue(int p_value) 
    {
        curValue = p_value;
        fillImage.fillAmount = (float)curValue / maxValue;
        remainText.text = curValue.ToString();
    }
}
