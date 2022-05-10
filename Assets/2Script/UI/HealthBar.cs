using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void SetMaxHealth(int _value)
    {
        slider.maxValue = _value;
        slider.value = _value;
    }

    public void SetHealth(int _value)
    {
        slider.value = _value;
    }
}
