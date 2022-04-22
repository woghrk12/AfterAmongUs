using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    private Rigidbody2D rigid;
    private Animator anim;
    private SpriteRenderer sprite;

    [SerializeField] private float moveSpeed;

    public Region playerRegion;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        var inst = Instantiate(sprite.material);
        sprite.material = inst;
    }

    private void Start()
    {
        sprite.material.SetColor("_PlayerColor", Color.green);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        var moveDir = Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f), 1f);

        anim.SetBool("isWalk", moveDir != Vector3.zero ? true : false);

        var curFlipX = sprite.flipX;
        sprite.flipX = (moveDir.x != 0) ? (moveDir.x < 0) : curFlipX;

        transform.position += moveDir * Time.deltaTime * moveSpeed;
    }
}
