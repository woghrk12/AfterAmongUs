using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    private Rigidbody2D rigid;
    private Animator anim;
    private SpriteRenderer sprite;
    private Camera mainCamera;

    [SerializeField] private List<PlayerWeapon> weapons;
    private PlayerWeapon equipWeapon;
    private int equipWeaponIndex = -1;

    private float hAxis;
    private float vAxis;
    private bool fDown;
    private bool sDown1;
    private bool sDown2;

    private float fireDelay;
    private bool isFireReady;

    [SerializeField] private float moveSpeed;

    [SerializeField] private int health;
    [SerializeField] private int ammo9mm;
    [SerializeField] private int ammo7mm;
    [SerializeField] private int ammo5mm;

    public Region playerRegion;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;

        var inst = Instantiate(sprite.material);
        sprite.material = inst;

        fireDelay = 0;
        isFireReady = true;
    }

    private void Start()
    {
        sprite.material.SetColor("_PlayerColor", Color.magenta);
    }

    private void Update()
    {
        GetInput();
        Look();
        Fire();
        Swap();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void GetInput()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        fDown = Input.GetButton("Fire");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
    }

    private void Move()
    {
        var moveDir = Vector3.ClampMagnitude(new Vector3(hAxis, vAxis, 0f), 1f);

        anim.SetBool("isWalk", moveDir != Vector3.zero ? true : false);

        transform.position += moveDir * Time.deltaTime * moveSpeed;
    }

    private void Look()
    {
        var mPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var oPosition = transform.position;

        sprite.flipX = mPosition.x < oPosition.x;
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
            fireDelay = 0;
        }
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
        }
    }

    private IEnumerator OnDamageCo(int _damage)
    {
        health -= _damage;
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
