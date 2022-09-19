using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    [SerializeField] private Animator anim = null;

    [SerializeField] private float moveSpeed = 0f;
    private Vector3 moveDir = Vector3.zero;

    public bool IsMove { get { return moveDir != Vector3.zero; } }

    private bool isLeft = false;
    public bool IsLeft 
    {
        private set 
        {
            isLeft = value;
            transform.localScale = new Vector3(isLeft ? -1f : 1f, 1f, 1f);
        }
        get { return isLeft; }
    }

    private void Update()
    {
        InputDirection();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void InputDirection()
    { 
        moveDir = Vector3.ClampMagnitude(new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f), 1f);
    }

    private void Move()
    {
        transform.position += moveDir * Time.deltaTime * moveSpeed;

        anim.SetBool("isWalk", IsMove);

        if (moveDir.x != 0f) IsLeft = moveDir.x < 0f;
    }
}
