using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    protected Animator anim;
    protected SpriteRenderer spriteRenderer;

    protected float hAxis;
    protected float vAxis;

    private bool canMove;
    public bool CanMove
    {
        set 
        {
            canMove = value;
            anim.SetBool("isWalk", value);
        }
        get { return canMove; }
    }

    protected bool isLeft;

    [SerializeField] protected float moveSpeed;
    protected Vector3 moveDir;

    [SerializeField] protected Button useButton;
    protected List<GameObject> canUseObject;
    protected GameObject interactObject;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        var t_instMat = Instantiate(spriteRenderer.material);
        spriteRenderer.material = t_instMat;

        canUseObject = new List<GameObject>();
    }

    protected virtual void Update()
    {
        GetInput();
    }

    protected virtual void FixedUpdate()
    {
        if (!canMove) return;

        Move();
    }

    protected virtual void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
    }

    protected virtual void Move()
    {
        moveDir = Vector3.ClampMagnitude(new Vector3(hAxis, vAxis, 0f), 1f);

        anim.SetBool("isWalk", moveDir != Vector3.zero ? true : false);

        transform.position += moveDir * Time.deltaTime * moveSpeed;
    }

    public void SetColor(EPlayerColor p_color)
    {
        spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(p_color));
    }

    public void SetAlpha(float p_value)
    {
        var t_color = spriteRenderer.color;
        t_color.a = p_value;
        spriteRenderer.color = t_color;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            canUseObject.Add(collision.gameObject);

            if (useButton.interactable) return;

            interactObject = collision.gameObject;
            useButton.interactable = true;
            useButton.onClick.AddListener(() =>
                {
                    collision.GetComponent<Interactable>().Use();
                });
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            canUseObject.Remove(collision.gameObject);
            useButton.onClick.RemoveAllListeners();

            if (canUseObject.Count <= 0)
            {
                interactObject = null;
                useButton.interactable = false;
            }

            if (interactObject == collision.gameObject)
            {
                interactObject = canUseObject[0];
                useButton.onClick.AddListener(() =>
                    {
                        collision.GetComponent<Interactable>().Use();
                    });
            }
        }
    }
}
