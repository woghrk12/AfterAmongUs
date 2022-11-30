using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayer : MonoBehaviour
{
    private Animator anim = null;

    [SerializeField] private CharacterMove moveController = null;
    [SerializeField] private CharacterColor colorController = null;
    [SerializeField] private CharacterTargeting targetingController = null;
    [SerializeField] private CharacterRader raderController = null;
    [SerializeField] private PlayerWeapon weaponController = null;

    private JoyStick joystick = null;

    private bool isReload = false;
    public bool IsReload { set { isReload = value; } get { return isReload; } }

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

    public Weapon EquipWeapon { get { return weaponController.EquipWeapon; } }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void InitPlayer()
    {
        joystick = UIManager.Instance.Joystick;
       
        weaponController.InitWeapon();
        raderController.SetRange(weaponController.EquipWeapon.Range);
        colorController.SetColor((int)GameManager.playerColor);

        CanMove = true;
    }

    private void Update()
    {
        if (!canMove) return;

        Targeting(raderController.Target);
        if (raderController.Target) Fire();
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
    
    public Tuple<int, int> Swap(int p_idx)
    {
        weaponController.ChangeWeapon(p_idx);
        var t_weapon = weaponController.EquipWeapon;
        raderController.SetRange(t_weapon.Range);
        var weaponInfo = new Tuple<int, int>(t_weapon.MaxBullet, t_weapon.CurBullet);
        return weaponInfo;
    }

    public IEnumerator Reload() => weaponController.Reload();
}
