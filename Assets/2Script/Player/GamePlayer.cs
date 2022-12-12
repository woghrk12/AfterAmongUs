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
    [SerializeField] private CharacterInteract interactController = null;
    [SerializeField] private Damagable healthController = null;

    private JoyStick joystick = null;
    private StatusUI statusUI = null;

    private bool isFire = false;
    public bool IsFire { set { isFire = value; } get { return isFire; } }

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

    private void OnEnable()
    {
        healthController.HitEvent += OnHit;
        healthController.DieEvent += OnDie;
    }

    private void OnDisable()
    {
        healthController.HitEvent -= OnHit;
        healthController.DieEvent -= OnDie;
    }

    public void InitPlayer(InGameUIGroup p_inGameUI)
    {
        joystick = UIManager.Instance.Joystick;
        statusUI = p_inGameUI.StatusUI;
        statusUI.SetHealthStatus(healthController);

        interactController.Init();
        weaponController.InitWeapon(statusUI);
        healthController.StartChecking(true);
        raderController.SetRange(weaponController.EquipWeapon.Range);
        colorController.SetPlayerColor((int)GameManager.playerColor);

        CanMove = true;
    }

    private void Update()
    {
        if (!canMove) return;

        Targeting(raderController.Target);
        if (isFire) Fire();
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

        weaponController.UseWeapon(statusUI.BulletStatus);
    }

    public void Swap(int p_idx, ControlSlider p_bulletStatlus)
    {
        weaponController.ChangeWeapon(p_idx, p_bulletStatlus);
        raderController.SetRange(weaponController.EquipWeapon.Range);
    }

    public IEnumerator Reload(ControlSlider p_bulletStatus) => weaponController.Reload(p_bulletStatus);

    private void OnHit()
    {
        StartCoroutine(HitEffect());
    }

    private IEnumerator HitEffect()
    {
        colorController.SetColor(Color.red);
        yield return Utilities.WaitForSeconds(0.05f);
        colorController.SetColor(Color.white);
    }

    private void OnDie()
    {
        weaponController.gameObject.SetActive(false);
        CanMove = false;
        anim.SetTrigger("Die");
    }

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
