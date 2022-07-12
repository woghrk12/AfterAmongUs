using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    [SerializeField] private Animator anim = null;
    [SerializeField] private SpriteRenderer spriteRender;

    [SerializeField] private JoyStick joyStick = null;
    [SerializeField] private KeyBoard keyBoard = null;

    private EControlType controlType = EControlType.KEYBOARD;

    [SerializeField] private float moveSpeed = 0f;
    private Vector3 moveDir = Vector3.zero;

    private bool isLeft = false;
    public bool IsLeft 
    {
        set 
        {
            isLeft = value;
            spriteRender.flipX = isLeft;
        }
        get { return isLeft; } }

    private bool canMove = false;
    public bool CanMove
    {
        set 
        {
            canMove = value;
            if(!canMove) anim.SetBool("isWalk", canMove);
        }
        get { return canMove; }
    }

    private void Start()
    {
        SetControlType(controlType);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F6))
            SetControlType(EControlType.JOYSTICK);
        SetMoveDirection();
    }

    private void FixedUpdate()
    {
        if (!canMove) return;

        Move(moveDir);
    }

    private void SetMoveDirection()
    {
        switch (controlType)
        {
            case EControlType.JOYSTICK:
                moveDir = joyStick.Direction;
                break;
            case EControlType.KEYBOARD:
                moveDir = keyBoard.Direction;
                break;
        }
    }

    public void SetControlType(EControlType p_controlType)
    {
        controlType = p_controlType;

        joyStick.gameObject.SetActive(controlType == EControlType.JOYSTICK);
        keyBoard.gameObject.SetActive(controlType == EControlType.KEYBOARD);
    }

    private void Move(Vector3 p_moveDir)
    {
        transform.position += p_moveDir * Time.deltaTime * moveSpeed;

        anim.SetBool("isWalk", p_moveDir != Vector3.zero ? true : false);

        if (p_moveDir.x != 0)
        {
            IsLeft = p_moveDir.x < 0;
        }
    }
}
