using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission : MonoBehaviour
{
    private Animator animCore = null;
    [SerializeField] private Region region = null;
    [SerializeField] private Animator animObj = null;

    public Region Region { get { return region; } }

    private void Awake()
    {
        animCore = GetComponent<Animator>();
    }

    public void TryEffect()
    {
        animCore.SetTrigger("Try");
    }

    public void ActivateEffect()
    {
        animCore.SetTrigger("On");
        animObj.SetTrigger("On");
    }

    public void DestroyEffect()
    {
        animCore.SetTrigger("Off");
        StartCoroutine(DestroyEffectCo());
    }

    private IEnumerator DestroyEffectCo()
    {
        var t_renderer = GetComponent<SpriteRenderer>();
        var t_timer = 0f;
        var t_totalTime = 1f;
        var t_color = t_renderer.color;

        while (t_timer < t_totalTime)
        {
            t_color.r -= 0.025f; t_color.g -= 0.025f; t_color.b -= 0.025f;
            t_renderer.color = t_color;
            t_timer += 0.05f;
            yield return Utilities.WaitForSeconds(0.05f);
        }
    }
}
