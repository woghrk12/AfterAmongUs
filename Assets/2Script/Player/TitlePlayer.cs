using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePlayer : MonoBehaviour
{
    private Animator anim = null;
    
    [SerializeField] private CharacterMove moveController = null;

    private JoyStick joystick = null;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        joystick = UIManager.Instance.Joystick;
    }

    private void OnEnable()
    {
        anim.SetTrigger("Spawn");
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move() => moveController.MoveCharacter(joystick.Direction, anim);
}
