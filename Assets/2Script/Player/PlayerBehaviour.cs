using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    protected Animator anim;
    protected SpriteRenderer spriteRenderer;

    protected float hAxis;
    protected float vAxis;

    public bool canMove;

    protected bool isLeft;

    [SerializeField] protected float moveSpeed;
    protected Vector3 moveDir;

    protected GameObject usingObject;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        var t_instMat = Instantiate(spriteRenderer.material);
        spriteRenderer.material = t_instMat;
    }

    protected virtual void Update()
    {
        GetInput();
    }

    protected virtual void FixedUpdate()
    {
        if (!canMove) return;

        Move();
    }

    protected virtual void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
    }

    protected virtual void Move()
    {
        moveDir = Vector3.ClampMagnitude(new Vector3(hAxis, vAxis, 0f), 1f);

        anim.SetBool("isWalk", moveDir != Vector3.zero ? true : false);

        transform.position += moveDir * Time.deltaTime * moveSpeed;
    }

    public void SetColor(EPlayerColor p_color)
    {
        spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(p_color));
    }

    public void SetAlpha(float p_value)
    {
        var t_color = spriteRenderer.color;
        t_color.a = p_value;
        spriteRenderer.color = t_color;
    }
}
