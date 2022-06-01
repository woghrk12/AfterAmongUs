using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer sprite;
    private Camera mainCamera;

    [SerializeField] private List<PlayerWeapon> weapons;
    private bool[] hasWeapons;
    private PlayerWeapon equipWeapon;
    private BulletType bulletType;
    private int equipWeaponIndex = -1;

    private float hAxis;
    private float vAxis;
    private bool fDown;
    private bool sDown1;
    private bool sDown2;
    private bool rDown;
    private bool uDown;
    private bool mDown;

    private bool isLeft;
    private bool isDie;
    private bool isFireReady;

    [SerializeField] private Transform playerAim;
    private float fireDelay;

    [SerializeField] private float moveSpeed;
    private Vector3 moveDir;

    private Vector3 mPosition;
    private Vector3 oPosition;
    private Vector3 direction;
    private float angle;

    [SerializeField] private int maxHealth;
    private int curHealth;
    
    private int ammo12Guage;
    private int ammo7MM;
    private int ammo5MM;
    
    [SerializeField] private ControlSlider healthBar;

    [SerializeField] private Text curAmmoText;
    [SerializeField] private Text totalAmmoText;
    [SerializeField] private Image ammoImage;

    [SerializeField] private Sprite ammo12GuageImage;
    [SerializeField] private Sprite ammo7MMImage;
    [SerializeField] private Sprite ammo5MMImage;

    public Region playerRegion;

    private GameObject usingObject;

    [SerializeField] private GameObject miniMap;

    [SerializeField] private GameObject itemAcqView;
    [SerializeField] private GameObject itemTextPrefab;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        mainCamera = Camera.main;

        var t_inst = Instantiate(sprite.material);
        sprite.material = t_inst;

        healthBar.SetMaxValue(maxHealth);
        curHealth = maxHealth;

        hasWeapons = new bool[weapons.Count];

        for (int i = 0; i < weapons.Count; i++)
            hasWeapons[i] = true;

        equipWeaponIndex = -1;
        
        isFireReady = true;
        isDie = false;

        fireDelay = 0;

        ammo12Guage = 50;
        ammo7MM = 50;
        ammo5MM = 50;
    }

    private void Start()
    {
        sprite.material.SetColor("_PlayerColor", Color.magenta);
    }

    private void Update()
    {
        if (isDie) return;

        GetInput();
        Targeting();
        Fire();
        Swap();
        Reload();
        Use();
        TurnOnOffMiniMap();
    }

    private void FixedUpdate()
    {
        if (isDie) return;

        Move();
    }

    private void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        fDown = Input.GetButton("Fire");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        rDown = Input.GetButtonDown("Reload");
        uDown = Input.GetButtonDown("Use");
        mDown = Input.GetButtonDown("Map");
    }

    private void Move()
    {
        moveDir = Vector3.ClampMagnitude(new Vector3(hAxis, vAxis, 0f), 1f);

        anim.SetBool("isWalk", moveDir != Vector3.zero ? true : false);

        transform.position += moveDir * Time.deltaTime * moveSpeed;
    }

    private void Targeting()
    {
        mPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        oPosition = transform.position;

        isLeft = mPosition.x < oPosition.x;

        sprite.flipX = isLeft;
        playerAim.localScale = new Vector3(isLeft ? -1f : 1f, 1f, 1f);
        direction = isLeft ? oPosition - mPosition : mPosition - oPosition;

        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (angle < -80f || angle > 80f)
            angle = Mathf.Clamp(angle, -80f, 80f);

        playerAim.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void ChangeColor(Color p_color)
    {
        sprite.color= p_color;
    }

    private void Fire()
    {
        if (equipWeapon == null) return;

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

        switch (bulletType)
        {
            case BulletType.FIVEMM:
                if (ammo5MM <= 0) return;
                ammo5MM -= equipWeapon.Reload(ammo5MM);
                totalAmmoText.text = ammo5MM.ToString();
                break;

            case BulletType.SEVENMM:
                if (ammo7MM <= 0) return;
                ammo7MM -= equipWeapon.Reload(ammo7MM);
                totalAmmoText.text = ammo7MM.ToString();
                break;

            case BulletType.TWELVEGAUGE:
                if (ammo12Guage <= 0) return;
                ammo12Guage -= equipWeapon.Reload(ammo12Guage);
                totalAmmoText.text = ammo12Guage.ToString();
                break;
        }

        curAmmoText.text = equipWeapon.curAmmo.ToString();
    }

    private void Swap()
    {
        if (!sDown1 && !sDown2) return;
        if (sDown1 && (equipWeaponIndex == 0 || !hasWeapons[0])) return;
        if (sDown2 && (equipWeaponIndex == 1 || !hasWeapons[1])) return;

        if (equipWeapon != null)
            equipWeapon.gameObject.SetActive(false);

        if (sDown1) equipWeaponIndex = 0;
        if (sDown2) equipWeaponIndex = 1;

        equipWeapon = weapons[equipWeaponIndex];
        equipWeapon.gameObject.SetActive(true);

        bulletType = equipWeapon.bulletType;

        curAmmoText.text = equipWeapon.curAmmo.ToString();
        switch (bulletType)
        {
            case BulletType.FIVEMM:
                ammoImage.sprite = ammo5MMImage;
                totalAmmoText.text = ammo5MM.ToString();
                break;

            case BulletType.SEVENMM:
                ammoImage.sprite = ammo7MMImage;
                totalAmmoText.text = ammo7MM.ToString();
                break;

            case BulletType.TWELVEGAUGE:
                ammoImage.sprite = ammo12GuageImage;
                totalAmmoText.text = ammo12Guage.ToString();
                break;
        }
    }

    private void Use()
    {
        if (!uDown) return;

        if (usingObject == null) return;

        var t_item = usingObject.GetComponent<ItemBox>().Use();

        var t_itemTextPrefab = Instantiate(itemTextPrefab, itemAcqView.transform).GetComponent<ItemText>();
        t_itemTextPrefab.SetText(t_item.itemType, t_item.num);
        
        switch (t_item.itemType)
        {
            case ItemType.AMMO12:
                ammo12Guage += t_item.num;
                if (equipWeapon.bulletType == BulletType.TWELVEGAUGE)
                    totalAmmoText.text = ammo12Guage.ToString();
                break;
            case ItemType.AMMO7:
                ammo7MM += t_item.num;
                if (equipWeapon.bulletType == BulletType.SEVENMM)
                    totalAmmoText.text = ammo7MM.ToString();
                break;
            case ItemType.AMMO5:
                ammo5MM += t_item.num;
                if (equipWeapon.bulletType == BulletType.FIVEMM)
                    totalAmmoText.text = ammo5MM.ToString();
                break;
            case ItemType.HEAL:
                curHealth += t_item.num;
                healthBar.SetValue(curHealth);
                break;
            case ItemType.GRENADE:
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
                    ChangeColor(new Color(1f, 1f, 1f, 0.3f));
                else
                    ChangeColor(new Color(1f, 1f, 1f, 0.6f));

                yield return new WaitForSeconds(0.2f);

                t_countTime++;
            }

            ChangeColor(new Color(1f, 1f, 1f, 1f));

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

        sprite.flipX = false;

        anim.SetTrigger("Die");
    }

    private void TurnOnOffMiniMap()
    {
        if (!mDown) return;

        miniMap.SetActive(!miniMap.activeSelf);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Region")
        {
            playerRegion = collision.GetComponent<Region>();
        }

        if (collision.tag == "EnemyBullet")
        {
            var damage = collision.GetComponent<Bullet>().damage;
            StartCoroutine(OnDamageCo(damage));
            ObjectPooling.ReturnObject(collision.gameObject);
        }

        if (collision.CompareTag("Item Box"))
            usingObject = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Region")
        {
            playerRegion = null;
        }

        if (collision.CompareTag("Item Box"))
            usingObject = null;
    }
}
