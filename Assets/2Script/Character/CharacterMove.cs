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

    public bool IsMove { get { return moveDir != Vector3.zero; } }

    private bool isLeft = false;
    public bool IsLeft 
    {
        set 
        {
            isLeft = value;
            transform.localScale = new Vector3(isLeft ? -1f : 1f, 1f, 1f);
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
        if (Input.GetKeyDown(KeyCode.F7))
            SetControlType(EControlType.KEYBOARD);

        SetMoveDirection();
    }

    private void FixedUpdate()
    {
        if (!CanMove) return;

        Move();
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

    private void Move()
    {
        transform.position += moveDir * Time.deltaTime * moveSpeed;

        anim.SetBool("isWalk", IsMove ? true : false);

        if (moveDir.x != 0)
        {
            IsLeft = moveDir.x < 0;
        }
    }
}
