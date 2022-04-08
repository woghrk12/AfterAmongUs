using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer sprite;

    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform target;

    private void Start()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        Chase(target.position);    
    }

    private void Chase(Vector3 target)
    {
        Vector3 moveDir = Vector3.ClampMagnitude((target - transform.position), 1f);
        
        bool curFlipX = sprite.flipX;
        sprite.flipX = (moveDir.x != 0) ? (moveDir.x < 0) : curFlipX;
        
        anim.SetBool("isWalk", true);
        
        transform.position += moveDir * Time.deltaTime * moveSpeed;
    }

}
