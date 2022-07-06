using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeLight : MonoBehaviour, IEffect
{
    private SpriteRenderer spriteRenderer;
    private Coroutine runningCo;

    private bool isRunningCo = false;

    [SerializeField] private Sprite[] lightSprites;
    [SerializeField] private Sprite tunrOffSprite;

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

        spriteRenderer.sprite = tunrOffSprite;
    }

    private IEnumerator RunEffectCo()
    {
        while (isRunningCo)
        {
            yield return Effect();
        }
    }

    private IEnumerator Effect()
    {
        var t_cnt = 0;
        var t_totalNum = lightSprites.Length;

        while (t_cnt < t_totalNum)
        {
            spriteRenderer.sprite = lightSprites[t_cnt];
            t_cnt++;

            yield return new WaitForSeconds(0.05f);
        }
    }
}
