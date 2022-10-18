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
    [SerializeField] private PlayerWeapon weaponController = null;

    private JoyStick joystick = null;

    private bool isReload = false;
    public bool IsReload { set { isReload = value; } get { return isReload; } }

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
        weaponController.InitWeapon();
        rader.SetRange(weaponController.EquipWeapon.Range);
        colorController.SetColor((int)GameManager.playerColor);
    }

    private void Update()
    {
        Targeting(rader.Target);

        if (Input.GetKey(KeyCode.F1))
            Fire();
    }

    private void FixedUpdate()
    {
        if (!canMove) return;

        Move();
    }
    
    private void Move() => moveController.MoveCharacter(joystick.Direction, anim);
    private void Targeting(Transform p_target) => targetingController.Targeting(p_target);
    private void Fire()
    {
        if (isReload) return;
        weaponController.UseWeapon();
    }
    
    public void Swap(int p_idx)
    {
        weaponController.ChangeWeapon(p_idx);
        rader.SetRange(weaponController.EquipWeapon.Range);
    }
    public IEnumerator Reload() => weaponController.Reload();
}
