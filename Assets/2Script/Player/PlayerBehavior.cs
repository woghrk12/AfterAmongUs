using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour
{
    private Rigidbody2D rigid;
    private Animator anim;
    private SpriteRenderer sprite;
    private Camera mainCamera;

    [SerializeField] private List<PlayerWeapon> weapons;
    private PlayerWeapon equipWeapon;
    private BulletType bulletType;
    private int equipWeaponIndex = -1;

    private float hAxis;
    private float vAxis;
    private bool fDown;
    private bool sDown1;
    private bool sDown2;
    private bool rDown;

    [SerializeField] private Transform playerAim;
    private float fireDelay;
    private bool isFireReady;

    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3 moveDir;

    [SerializeField] private int maxHealth;
    private int curHealth;
    
    [SerializeField] private int ammo12Guage;
    [SerializeField] private int ammo7MM;
    [SerializeField] private int ammo5MM;
    
    [SerializeField] private HealthBar healthBar;

    [SerializeField] private Text curAmmoText;
    [SerializeField] private Text totalAmmoText;
    [SerializeField] private Image ammoImage;

    [SerializeField] private Sprite ammo12GuageImage;
    [SerializeField] private Sprite ammo7MMImage;
    [SerializeField] private Sprite ammo5MMImage;

    private bool isDie;

    public Region playerRegion;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        mainCamera = Camera.main;

        var inst = Instantiate(sprite.material);
        sprite.material = inst;

        healthBar.SetMaxHealth(maxHealth);
        curHealth = maxHealth;

        fireDelay = 0;
        isFireReady = true;

        isDie = false;
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
    }

    private void FixedUpdate()
    {
        if (isDie) return;

        Move();
    }

    private void GetInput()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        fDown = Input.GetButton("Fire");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        rDown = Input.GetButtonDown("Reload");
    }

    private void Move()
    {
        moveDir = Vector3.ClampMagnitude(new Vector3(hAxis, vAxis, 0f), 1f);

        anim.SetBool("isWalk", moveDir != Vector3.zero ? true : false);

        transform.position += moveDir * Time.deltaTime * moveSpeed;
    }

    private void Targeting()
    {
        var mPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var oPosition = transform.position;

        var isLeft = mPosition.x < oPosition.x;

        sprite.flipX = isLeft;
        playerAim.localScale = new Vector3(isLeft ? -1f : 1f, 1f, 1f);
        var direction = isLeft ? oPosition - mPosition : mPosition - oPosition;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (angle < -80f || angle > 80f)
            angle = Mathf.Clamp(angle, -80f, 80f);

        playerAim.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void ChangeColor(Color _color)
    {
        sprite.color= _color;
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
        if (sDown1 && equipWeaponIndex == 0) return;
        if (sDown2 && equipWeaponIndex == 1) return;
        
        int weaponIndex = -1;
        if (sDown1) weaponIndex = 0;
        if (sDown2) weaponIndex = 1;

        if (sDown1 || sDown2)
        {
            if (equipWeapon != null)
                equipWeapon.gameObject.SetActive(false);

            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex];
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
    }

    private IEnumerator OnDamageCo(int _damage)
    {
        curHealth -= _damage;

        healthBar.SetHealth(curHealth);

        if (curHealth > 0)
        {
            gameObject.layer = 3;
            int countTime = 0;

            while (countTime < 10)
            {
                if (countTime % 2 == 0)
                    ChangeColor(new Color(1f, 1f, 1f, 0.3f));
                else
                    ChangeColor(new Color(1f, 1f, 1f, 0.6f));

                yield return new WaitForSeconds(0.2f);

                countTime++;
            }

            ChangeColor(new Color(1f, 1f, 1f, 1f));

            gameObject.layer = 7;
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
        gameObject.layer = 8;
        isDie = true;

        sprite.flipX = false;

        anim.SetTrigger("Die");
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
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Region")
        {
            playerRegion = null;
        }
    }
}
