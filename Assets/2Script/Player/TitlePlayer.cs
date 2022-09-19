using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePlayer : MonoBehaviour
{
    private Animator anim = null;
    [SerializeField] private CharacterMove moveController = null;
    private JoyStick joyStick = null;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        joyStick = UIManager.Instance.JoyStick;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move() => moveController.MoveCharacter(joyStick.Direction, anim);
}
