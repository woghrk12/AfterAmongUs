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

    private float hAxis;
    private float vAxis;
    

    [SerializeField] private float size;
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

        Look();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void GetInput()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");

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

    private void Swap()
    { 
    
    }
}
