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
    }

}
