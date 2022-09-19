using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePlayer : MonoBehaviour
{
    private Animator anim = null;
    
    [SerializeField] private CharacterMove moveController = null;

    private JoyStick joystick = null;

    private bool canMove = false;
    public bool CanMove
    {
        set 
        {
            canMove = value;
            if (!canMove) moveController.MoveCharacter(Vector3.zero, anim);
        }
        get { return canMove; }
    }
    
    private void Awake()
    {
        anim = GetComponent<Animator>();

        joystick = UIManager.Instance.Joystick;
    }

    private void OnEnable()
    {
        CanMove = false;
        anim.SetTrigger("Spawn");
    }

    private void FixedUpdate()
    {
        if (!canMove) return;

        Move();
    }

    private void Move() => moveController.MoveCharacter(joystick.Direction, anim);
}
