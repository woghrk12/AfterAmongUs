using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private EItemType itemType;
    private int num;

    [SerializeField] private SpriteRenderer itemImage;
    [SerializeField] private SpriteRenderer outlineSprite;

    private CircleCollider2D circleCollider;

    [SerializeField] private Sprite ammo5Image;
    [SerializeField] private Sprite ammo7Image;
    [SerializeField] private Sprite ammo9Image;
    [SerializeField] private Sprite ammo12Image;
    [SerializeField] private Sprite healImage;
    [SerializeField] private Sprite pistolImage;
    [SerializeField] private Sprite rifleImage;
    [SerializeField] private Sprite shotgunImage;

    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
    }

    public void GetItemValue(out EItemType p_itemType, out int p_num)
    {
        p_itemType = itemType;
        p_num = num;

        circleCollider.enabled = false;
    }

    public void SetItem(EItemType p_itemType, int p_num, Vector3 p_originPos)
    {
        itemType = p_itemType;
        num = p_num;
        transform.position = p_originPos;

        switch (p_itemType)
        {
            case EItemType.AMMO5:
                transform.localScale = new Vector3(0.4f, 0.4f, 1f);
                outlineSprite.sprite = itemImage.sprite = ammo5Image;
                break;

            case EItemType.AMMO7:
                transform.localScale = new Vector3(0.4f, 0.4f, 1f);
                outlineSprite.sprite = itemImage.sprite = ammo7Image;
                break;

            case EItemType.AMMO12:
                transform.localScale = new Vector3(0.4f, 0.4f, 1f);
                outlineSprite.sprite = itemImage.sprite = ammo12Image;
                break;

            case EItemType.AMMO9:
                transform.localScale = new Vector3(0.4f, 0.4f, 1f);
                outlineSprite.sprite = itemImage.sprite = ammo9Image;
                break;

            case EItemType.HEAL:
                transform.localScale = new Vector3(0.03f, 0.03f, 1f);
                outlineSprite.sprite = itemImage.sprite = healImage;
                break;

            case EItemType.WEAPON:
                if (num == (int)EWeaponType.RIFLE)
                {
                    transform.localScale = new Vector3(0.3f, 0.5f, 1f);
                    outlineSprite.sprite = itemImage.sprite = rifleImage;
                }
                else if (num == (int)EWeaponType.SHOTGUN)
                {
                    transform.localScale = new Vector3(0.3f, 0.5f, 1f);
                    outlineSprite.sprite = itemImage.sprite = shotgunImage;
                }
                else if (num == (int)EWeaponType.PISTOL)
                {
                    transform.localScale = new Vector3(0.2f, 0.2f, 1f);
                    outlineSprite.sprite = itemImage.sprite = pistolImage;
                }
                break;
        }

        var t_dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 1f).normalized;
        StartCoroutine(SpreadItem(t_dir));
    }

    private IEnumerator SpreadItem(Vector3 p_dir)
    {
        var t_timer = 1f;
        var t_totalTime = 1f;
        
        while (t_timer >= 0f)
        {
            var t_speed = Mathf.Lerp(0f, 1f, t_timer / t_totalTime);
            transform.position += p_dir * Time.deltaTime * t_speed;
            t_timer -= Time.deltaTime;
            yield return null;
        }

        circleCollider.enabled = true;
    }
}
