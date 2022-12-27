using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointLight : MonoBehaviour, IEffect
{
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private Sprite[] lightSprites = null;

    public void StartEffect() => StartCoroutine(Effect());

    private IEnumerator Effect()
    {
        var t_cnt = 0;
        var t_totalNum = lightSprites.Length;

        while (true)
        {
            spriteRenderer.sprite = lightSprites[t_cnt];
            t_cnt = t_cnt < t_totalNum - 2 ? t_cnt + 1 : 0;
            yield return Utilities.WaitForSeconds(0.05f);
        }
    }
}
