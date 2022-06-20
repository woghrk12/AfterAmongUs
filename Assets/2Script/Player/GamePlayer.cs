using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayer : PlayerBehaviour
{
    private Camera mainCamera;

    [SerializeField] private List<PlayerWeapon> weapons;
    private bool[] hasWeapons;
    private PlayerWeapon equipWeapon;
    private EBulletType bulletType;
    private int equipWeaponIndex = -1;

    private bool fDown;
    private bool sDown1;
    private bool sDown2;
    private bool rDown;
    private bool mDown;

    private bool isDie;
    private bool isFireReady;
    private bool isReloadReady;

    [SerializeField] private Transform playerAim;
    private float fireDelay;

    private Coroutine reloadCo;

    private Vector3 mPosition;
    private Vector3 oPosition;
    private Vector3 direction;
    private float angle;

    [SerializeField] private ControlSlider healthBar;
    [SerializeField] private int maxHealth;
    private int curHealth;

    private int ammo12Guage;
    private int ammo7MM;
    private int ammo5MM;

    [SerializeField] private Text curAmmoText;
    [SerializeField] private Text totalAmmoText;
    [SerializeField] private Image ammoImage;

    [SerializeField] private Sprite ammo12GuageImage;
    [SerializeField] private Sprite ammo7MMImage;
    [SerializeField] private Sprite ammo5MMImage;

    public Region playerRegion;

    [SerializeField] private GameObject miniMap;

    [SerializeField] private GameObject itemAcqView;
    [SerializeField] private GameObject itemTextPrefab;

    [SerializeField] private Button useButton;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        mainCamera = Camera.main;

        var t_inst = Instantiate(spriteRenderer.material);
        spriteRenderer.material = t_inst;

        healthBar.SetMaxValue(maxHealth);
        curHealth = maxHealth;

        hasWeapons = new bool[weapons.Count];

        for (int i = 0; i < weapons.Count; i++)
            hasWeapons[i] = true;

        equipWeaponIndex = -1;

        isFireReady = true;
        isDie = false;
        isReloadReady = true;

        fireDelay = 0f;

        ammo12Guage = 50;
        ammo7MM = 50;
        ammo5MM = 50;
    }

    private void Start()
    {
        spriteRenderer.material.SetColor("_PlayerColor", Color.magenta);
    }

    protected override void Update()
    {
        if (isDie) return;

        base.Update();

        Targeting();
        Fire();
        Swap();
        Reload();
        TurnOnOffMiniMap();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void GetInput()
    {
        base.GetInput();

        fDown = Input.GetButton("Fire");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        rDown = Input.GetButtonDown("Reload");
        mDown = Input.GetButtonDown("Map");
    }

    private void Targeting()
    {
        mPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        oPosition = transform.position;

        isLeft = mPosition.x < oPosition.x;

        spriteRenderer.flipX = isLeft;
        playerAim.localScale = new Vector3(isLeft ? -1f : 1f, 1f, 1f);
        direction = isLeft ? oPosition - mPosition : mPosition - oPosition;

        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (angle < -80f || angle > 80f)
            angle = Mathf.Clamp(angle, -80f, 80f);

        playerAim.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void Fire()
    {
        if (equipWeapon == null) return;
        if (!isReloadReady) return;

        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        if (fDown && isFireReady)
        {
            equipWeapon.GetComponent<PlayerWeapon>().Shot();
            curAmmoText.text = equipWeapon.curAmmo.ToString();
            fireDelay = 0;
        }
    }

    private void Reload()
    {
        if (equipWeapon == null) return;
        if (!rDown) return;
        if (!isReloadReady) return;

        reloadCo = StartCoroutine(ReloadCo(equipWeapon.bulletType, equipWeapon.reloadTime));
    }

    private IEnumerator ReloadCo(EBulletType p_bulletType, float p_reloadTime)
    {
        isReloadReady = false;

        yield return new WaitForSeconds(p_reloadTime);

        switch (p_bulletType)
        {
            case EBulletType.FIVEMM:
                if (ammo5MM <= 0) break;
                ammo5MM -= equipWeapon.Reload(ammo5MM);
                totalAmmoText.text = ammo5MM.ToString();
                break;

            case EBulletType.SEVENMM:
                if (ammo7MM <= 0) break;
                ammo7MM -= equipWeapon.Reload(ammo7MM);
                totalAmmoText.text = ammo7MM.ToString();
                break;

            case EBulletType.TWELVEGAUGE:
                if (ammo12Guage <= 0) break;
                ammo12Guage -= equipWeapon.Reload(ammo12Guage);
                totalAmmoText.text = ammo12Guage.ToString();
                break;
        }
        curAmmoText.text = equipWeapon.curAmmo.ToString();

        reloadCo = null;
        isReloadReady = true;
    }

    private void Swap()
    {
        if (!sDown1 && !sDown2) return;
        if (sDown1 && (equipWeaponIndex == 0 || !hasWeapons[0])) return;
        if (sDown2 && (equipWeaponIndex == 1 || !hasWeapons[1])) return;

        if (reloadCo != null)
        {
            StopCoroutine(reloadCo);
            isReloadReady = true;
        }
        if (equipWeapon != null) equipWeapon.gameObject.SetActive(false);

        if (sDown1) equipWeaponIndex = 0;
        if (sDown2) equipWeaponIndex = 1;

        equipWeapon = weapons[equipWeaponIndex];
        equipWeapon.gameObject.SetActive(true);

        bulletType = equipWeapon.bulletType;

        switch (bulletType)
        {
            case EBulletType.FIVEMM:
                ammoImage.sprite = ammo5MMImage;
                totalAmmoText.text = ammo5MM.ToString();
                break;

            case EBulletType.SEVENMM:
                ammoImage.sprite = ammo7MMImage;
                totalAmmoText.text = ammo7MM.ToString();
                break;

            case EBulletType.TWELVEGAUGE:
                ammoImage.sprite = ammo12GuageImage;
                totalAmmoText.text = ammo12Guage.ToString();
                break;
        }

        curAmmoText.text = equipWeapon.curAmmo.ToString();
    }

    private void Use()
    {
        if (usingObject == null) return;

        var t_item = usingObject.GetComponent<ItemBox>().Use();
        
        var t_itemTextPrefab = Instantiate(itemTextPrefab, itemAcqView.transform).GetComponent<ItemText>();
        t_itemTextPrefab.SetText(t_item.itemType, t_item.num);

        switch (t_item.itemType)
        {
            case EItemType.AMMO12:
                ammo12Guage += t_item.num;
                if (equipWeapon.bulletType == EBulletType.TWELVEGAUGE)
                    totalAmmoText.text = ammo12Guage.ToString();
                break;
            case EItemType.AMMO7:
                ammo7MM += t_item.num;
                if (equipWeapon.bulletType == EBulletType.SEVENMM)
                    totalAmmoText.text = ammo7MM.ToString();
                break;
            case EItemType.AMMO5:
                ammo5MM += t_item.num;
                if (equipWeapon.bulletType == EBulletType.FIVEMM)
                    totalAmmoText.text = ammo5MM.ToString();
                break;
            case EItemType.HEAL:
                curHealth += t_item.num;
                healthBar.SetValue(curHealth);
                break;
            case EItemType.GRENADE:
                break;
            default:
                break;
        }
    }

    private IEnumerator OnDamageCo(int p_damage)
    {
        curHealth -= p_damage;
        healthBar.SetValue(curHealth);

        if (curHealth > 0)
        {
            gameObject.layer = 7;
            int t_countTime = 0;

            while (t_countTime < 10)
            {
                if (t_countTime % 2 == 0)
                    SetAlpha(0.3f);
                else
                    SetAlpha(0.6f);

                yield return new WaitForSeconds(0.2f);

                t_countTime++;
            }

            SetAlpha(1f);

            gameObject.layer = 6;
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        if (equipWeapon != null)
        {
            equipWeapon.gameObject.SetActive(false);
        }

        moveSpeed = 0;
        gameObject.layer = 7;
        isDie = true;
        CanMove = false;

        spriteRenderer.flipX = false;

        anim.SetTrigger("Die");
    }

    private void TurnOnOffMiniMap()
    {
        if (!mDown) return;

        miniMap.SetActive(!miniMap.activeSelf);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Region"))
        {
            playerRegion = collision.GetComponent<Region>();
        }

        if (collision.CompareTag("EnemyBullet"))
        {
            var damage = collision.GetComponent<Bullet>().damage;
            StartCoroutine(OnDamageCo(damage));
            ObjectPooling.ReturnObject(collision.gameObject);
        }

        if (collision.CompareTag("Interactable"))
        {
            usingObject = collision.gameObject;
            useButton.interactable = true;
            useButton.onClick.AddListener(() =>
                {
                    Use();
                });
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Region"))
        {
            playerRegion = null;
        }

        if (collision.CompareTag("Interactable"))
        {
            usingObject = null;
            useButton.interactable = false;
            useButton.onClick.RemoveAllListeners();
        }
    }
}
