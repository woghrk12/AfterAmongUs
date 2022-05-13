using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void SetMaxValue(int _value)
    {
        slider.maxValue = _value;
        slider.value = _value;
    }

    public void SetValue(int _value)
    {
        slider.value = _value;
    }
}
