using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steam : MonoBehaviour, IEffect
{
    private SpriteRenderer spriteRenderer;
    private Coroutine runningCo;

    private bool isRunningCo = false;

    [SerializeField] private Sprite[] steamSprites;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void StartEffect()
    {
        isRunningCo = true;
        runningCo = StartCoroutine(RunEffectCo());
    }

    public void StopEffect()
    {
        if (isRunningCo) StopCoroutine(runningCo);
        isRunningCo = false;
    }


    private IEnumerator RunEffectCo()
    {
        while (isRunningCo)
        {
            yield return new WaitForSeconds(Random.Range(1f, 2f));

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

            yield return new WaitForSeconds(0.05f);
        }

        spriteRenderer.sprite = null;
    }
}
