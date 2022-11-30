using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] private Image progressBar = null;

    public void SetProgress(float p_value)
    {
        progressBar.fillAmount = p_value;
    }
}
