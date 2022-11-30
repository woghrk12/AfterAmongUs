using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomUI : UIBase
{
    [SerializeField] private Animator panel = null;

    protected override IEnumerator OnEffect()
    {
        panel.SetTrigger("On");
        yield return Utilities.WaitForSeconds(EffectTime);
    }

    protected override IEnumerator OffEffect()
    {
        panel.SetTrigger("Off");
        yield return Utilities.WaitForSeconds(EffectTime);
    }
}
