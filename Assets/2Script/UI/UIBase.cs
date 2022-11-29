using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    [SerializeField] private float effectTime = 0f;
    public float EffectTime { get { return effectTime; } }

    public virtual IEnumerator OnUI(bool p_isEffect = false)
    {
        gameObject.SetActive(true);
        if (p_isEffect) yield return OnEffect();
    }
    public virtual IEnumerator OffUI(bool p_isEffect = false)
    {
        if (p_isEffect) yield return OffEffect();
        gameObject.SetActive(false);
    }

    protected virtual IEnumerator OnEffect() { yield return null; }
    protected virtual IEnumerator OffEffect() { yield return null; }
}
