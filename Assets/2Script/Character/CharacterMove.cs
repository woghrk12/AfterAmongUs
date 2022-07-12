using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CharacterMove : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private SpriteRenderer spriteRender;

    [SerializeField] private JoyStick joyStick;

    private EControlType controlType = EControlType.JOYSTICK;

    [SerializeField] private float moveSpeed;

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

    private void FixedUpdate()
    {
        if (!canMove) return;

        MoveByControlType();
    }

    private void MoveByControlType()
    {
        switch (controlType)
        {
            case EControlType.JOYSTICK:
                Move(joyStick.Direction);
                break;
        }
    }

    public void SetControlType(EControlType p_controlType)
    {
        controlType = p_controlType;

        if (controlType != EControlType.JOYSTICK)
            joyStick.gameObject.SetActive(false);
    }

    private void Move(Vector3 p_moveDir)
    {
        if (!canMove) return;

        transform.position += p_moveDir * Time.deltaTime * moveSpeed;

        anim.SetBool("isWalk", p_moveDir != Vector3.zero ? true : false);

        if (p_moveDir.x != 0)
        {
            isLeft = p_moveDir.x < 0;
            spriteRender.flipX = isLeft;
        }
    }
}
