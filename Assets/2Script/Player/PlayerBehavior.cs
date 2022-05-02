using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    private Rigidbody2D rigid;
    private Animator anim;
    private SpriteRenderer sprite;
    private Camera mainCamera;

    [SerializeField] private List<GameObject> weapons;
    private GameObject equipWeapon;
    private int equipWeaponIndex = -1;

    private float hAxis;
    private float vAxis;
    private bool fDown;
    private bool sDown1;
    private bool sDown2;

    [SerializeField] private int health;
    [SerializeField] private float moveSpeed;
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

        if (fDown)
            equipWeapon.GetComponent<PlayerWeapon>().Shot();
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
                equipWeapon.SetActive(false);

            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex];
            equipWeapon.SetActive(true);
        }
    }
}
