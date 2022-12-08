using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    [SerializeField] private Sprite[] sideSprites = null;
    [SerializeField] private Sprite[] frontSprites = null;
    [SerializeField] private SpriteRenderer side = null;
    [SerializeField] private SpriteRenderer front = null;

    public void ShowFlash() => StartCoroutine(FlashCo());

    public void SetFlash(Vector3 p_firePosition, Vector3 p_direction)
    {
        transform.position = p_firePosition;
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(p_direction.y, p_direction.x) * Mathf.Rad2Deg, Vector3.forward);
        side.sprite = sideSprites[Random.Range(0, sideSprites.Length)];
        front.sprite = frontSprites[Random.Range(0, frontSprites.Length)];
    }

    private IEnumerator FlashCo()
    {
        yield return Utilities.WaitForSeconds(0.1f);
        ObjectPoolingManager.ReturnObject(gameObject);
    }
}
