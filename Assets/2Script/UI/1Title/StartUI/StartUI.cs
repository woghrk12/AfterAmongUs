using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUI : UIBase
{
    [SerializeField] private Animator titleImage = null;
    [SerializeField] private Animator gameStartButton = null;

    protected override IEnumerator OnEffect()
    {
        titleImage.SetTrigger("On");
        gameStartButton.SetTrigger("On");
        yield return Utilities.WaitForSeconds(EffectTime);
    }

    protected override IEnumerator OffEffect()
    {
        titleImage.SetTrigger("Off");
        gameStartButton.SetTrigger("Off");
        yield return Utilities.WaitForSeconds(EffectTime);
    }
}
