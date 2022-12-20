using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steam : MonoBehaviour, IEffect
{
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private Sprite[] steamSprites = null;

    public void StartEffect() => StartCoroutine(RunEffect());

    private IEnumerator RunEffect()
    {
        while (true)
        {
            yield return Utilities.WaitForSeconds(Random.Range(1, 3));

            StartCoroutine(Effect());
        }
    }

    private IEnumerator Effect()
    {
        var t_cnt = 0;
        var t_totalNum = steamSprites.Length;

        while (t_cnt < t_totalNum)
        {
            spriteRenderer.sprite = steamSprites[t_cnt];
            t_cnt++;

            yield return Utilities.WaitForSeconds(0.05f);
        }

        spriteRenderer.sprite = null;
    }
}
