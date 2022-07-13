using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    protected Animator anim;
    protected SpriteRenderer spriteRenderer;

    protected CharacterMove moveController;

    public bool CanMove { set { moveController.CanMove = value; } get { return moveController.CanMove; } }
    public bool IsLeft { set { moveController.IsLeft = value; } get { return moveController.IsLeft; } }

    [SerializeField] protected Button useButton;
    protected List<GameObject> canUseObject;
    protected GameObject interactObject;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        var t_instMat = Instantiate(spriteRenderer.material);
        spriteRenderer.material = t_instMat;

        moveController = GetComponent<CharacterMove>();

        canUseObject = new List<GameObject>();
    }

    public void SetColor(EPlayerColor p_color)
    {
        spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(p_color));
    }

    protected void SetAlpha(float p_value)
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
                    collision.GetComponent<IInteractable>().Use();
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
                        collision.GetComponent<IInteractable>().Use();
                    });
            }
        }
    }
}
