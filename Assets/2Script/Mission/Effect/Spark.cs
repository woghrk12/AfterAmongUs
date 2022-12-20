using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spark : MonoBehaviour, IEffect
{ 
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private Sprite[] sparkSprites = null;
    private int numSpark = 5;

    public void StartEffect() => StartCoroutine(RunEffect());
    
    private IEnumerator RunEffect()
    {
        while (true)
        {
            yield return Utilities.WaitForSeconds(Random.Range(2, 4));

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
