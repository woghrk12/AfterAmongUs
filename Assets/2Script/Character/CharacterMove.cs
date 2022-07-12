using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private SpriteRenderer spriteRender;

    [SerializeField] private JoyStick joyStick = null;
    [SerializeField] private KeyBoard keyBoard = null;

    private EControlType controlType = EControlType.KEYBOARD;

    [SerializeField] private float moveSpeed;
    private Vector3 moveDir;

    private bool isLeft;
    public bool IsLeft { get { return isLeft; } }

    private bool canMove = true;
    public bool CanMove
    {
        set 
        {
            canMove = value;
            if(!canMove) anim.SetBool("isWalk", canMove);
        }
        get { return canMove; }
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        SetControlType(controlType);
    }

    private void Update()
    {
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
        keyBoard.IsKeyBoard = controlType == EControlType.KEYBOARD;
    }

    private void Move(Vector3 p_moveDir)
    {
        transform.position += p_moveDir * Time.deltaTime * moveSpeed;

        anim.SetBool("isWalk", p_moveDir != Vector3.zero ? true : false);

        if (p_moveDir.x != 0)
        {
            isLeft = p_moveDir.x < 0;
            spriteRender.flipX = isLeft;
        }
    }
}
