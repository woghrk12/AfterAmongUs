using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spark : MonoBehaviour, IEffect
{
    private SpriteRenderer spriteRenderer;
    private Coroutine runningCo;

    private bool isRunningCo = false;

    [SerializeField] private Sprite[] sparkSprites;
    private int numSpark = 5;

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
            yield return new WaitForSeconds(Random.Range(2f, 4f));

            StartCoroutine(Effect());
        }
    }

    private IEnumerator Effect()
    {
        var t_cnt = 0;
        var t_startNum = Random.Range(0, sparkSprites.Length - numSpark + 1);

        while (t_cnt < numSpark)
        {
            spriteRenderer.sprite = sparkSprites[t_startNum + t_cnt];
            t_cnt++;

            yield return null;
        }

        spriteRenderer.sprite = null;
    }
}
