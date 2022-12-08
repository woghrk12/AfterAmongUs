using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite = null;

    public void OnHit(int p_damage)
    {
        Debug.Log(p_damage);
        StartCoroutine(HitEffect());
    }

    private IEnumerator HitEffect()
    {
        if (!sprite) yield break;

        sprite.color = Color.red;
        yield return Utilities.WaitForSeconds(0.05f);
        sprite.color = Color.white;
    }
}
