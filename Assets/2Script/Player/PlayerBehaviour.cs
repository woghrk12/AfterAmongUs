using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    protected CharacterMove moveController = null;
    protected CharacterColor colorController = null;

    public bool CanMove { set { moveController.CanMove = value; } }
    public bool IsLeft { set { moveController.IsLeft = value; } }

    protected virtual void Awake()
    {
        moveController = GetComponent<CharacterMove>();
        colorController = GetComponent<CharacterColor>();
    }

    protected virtual void Start()
    {
        moveController.SetControlType(GameManager.controlType);
        colorController.SetColor(GameManager.playerColor);
    }

    public void SetColor(EPlayerColor p_color) => colorController.SetColor(p_color);
}
