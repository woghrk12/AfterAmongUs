using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSelectPanel : MonoBehaviour
{
    [SerializeField] private TitlePlayer titlePlayer = null;
    [SerializeField] private Image characterPreview = null;
    [SerializeField] private List<Button> colorSelectButtons = new List<Button>();

    private void OnEnable()
    {
        var t_color = (int)GameManager.playerColor;
        colorSelectButtons[t_color].interactable = false;
        SetPreviewImageColor(t_color);
    }

    public void OnClickColorButton(int p_color)
    {
        var t_oldColor = (int)GameManager.playerColor;
        colorSelectButtons[t_oldColor].interactable = true;
        
        SetPreviewImageColor(p_color);

        GameManager.playerColor = (EPlayerColor)p_color;
        titlePlayer.SetPlayerColor(p_color);
        colorSelectButtons[p_color].interactable = false;
    }

    private void SetPreviewImageColor(int p_color) 
        => characterPreview.material.SetColor("_PlayerColor", PlayerColor.GetColor((EPlayerColor)p_color));
}
