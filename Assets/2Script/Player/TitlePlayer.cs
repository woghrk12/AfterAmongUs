using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitlePlayer : PlayerBehaviour
{
    private bool uDown;

    [SerializeField] private GameObject playerUI;
    [SerializeField] private Button useButton;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        var t_instMat = Instantiate(spriteRenderer.material);
        spriteRenderer.material = t_instMat;

        canMove = false;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void GetInput()
    {
        base.GetInput();

        uDown = Input.GetButtonDown("Use");
    }

    protected override void Move()
    {
        base.Move();

        if(moveDir.x != 0f)
            spriteRenderer.flipX = moveDir.x < 0;
    }

    public void SpawnPlayer(bool p_isFlipX)
    {
        spriteRenderer.enabled = true;
        spriteRenderer.flipX = p_isFlipX;
        
        anim.SetTrigger("Spawn");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            useButton.interactable = true;
            useButton.onClick.AddListener(() =>
                {
                    playerUI.SetActive(false);
                    collision.GetComponentInParent<Laptop>().Use();
                });
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            useButton.interactable = false;
            useButton.onClick.RemoveAllListeners();
        }
    }
}
