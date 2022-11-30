using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePlayer : MonoBehaviour
{
    private Animator anim = null;
    
    [SerializeField] private CharacterMove moveController = null;
    [SerializeField] private CharacterInteract interactController = null;
    [SerializeField] private CharacterColor colorController = null;

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
    }

    public void InitPlayer()
    {
        CanMove = false;

        joystick = UIManager.Instance.Joystick;
        interactController.Init();
        colorController.SetColor((int)GameManager.playerColor);

        anim.SetTrigger("Spawn");
    }

    private void FixedUpdate()
    {
        if (!canMove) return;

        Move();
    }

    private void Move() => moveController.MoveCharacter(joystick.Direction, anim);

    public void SetPlayerColor(int p_color) => colorController.SetColor(p_color);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Interactable")
            interactController.AddObj(collision.GetComponentInParent<IInteractable>());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Interactable")
            interactController.RemoveObj(collision.GetComponentInParent<IInteractable>());
    }
}
