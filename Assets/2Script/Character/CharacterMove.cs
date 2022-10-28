using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0f;

    private bool isLeft = false;
    public bool IsLeft 
    {
        set 
        {
            isLeft = value;
            transform.localScale = new Vector3(isLeft ? -1f : 1f, 1f, 1f);
        }
        get { return isLeft; }
    }

    public void MoveCharacter(Vector3 p_moveDir, Animator p_anim) => Move(p_moveDir, p_anim);

    private void Move(Vector3 p_moveDir, Animator p_anim = null)
    {
        transform.position += p_moveDir * Time.deltaTime * moveSpeed;
        if (p_moveDir.x != 0f) IsLeft = p_moveDir.x < 0f;
        if (!p_anim) return;
        p_anim.SetBool("isWalk", p_moveDir != Vector3.zero);
    }
}
