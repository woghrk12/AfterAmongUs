using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayer : MonoBehaviour
{
    private Animator anim = null;

    [SerializeField] private CharacterMove moveController = null;
    [SerializeField] private CharacterColor colorController = null;
    [SerializeField] private CharacterTargeting targetingController = null;
    [SerializeField] private CharacterRader rader = null;

    private JoyStick joystick = null;

    private bool canMove = true;
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

    private void Start()
    {
        rader.SetRange(2f);
        colorController.SetColor((int)GameManager.playerColor);
    }

    private void Update()
    {
        Targeting(rader.Target);
    }

    private void FixedUpdate()
    {
        if (!canMove) return;

        Move();
    }
    
    private void Move() => moveController.MoveCharacter(joystick.Direction, anim);
    private void Targeting(Transform p_target) => targetingController.Targeting(p_target);
}
