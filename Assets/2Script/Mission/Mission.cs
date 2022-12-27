using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Mission : MonoBehaviour
{
    [SerializeField] private Animator animCore = null;
    [SerializeField] private ERegion region = ERegion.END;
    [SerializeField] private int missionTime = 0;

    [SerializeField] private Animator animObj = null;
    [SerializeField] private GameObject effectParent = null;
    private IEffect[] effectObj = null;
    [SerializeField] private Damagable hitController = null;

    public ERegion Region { get { return region; } }
    public int MissionTime { get { return missionTime; } }
    public Damagable HitController { get { return hitController; } }

    private void Awake()
    {
        if (!effectParent) return;
        
        effectObj = effectParent.GetComponentsInChildren<IEffect>();
    }

    private void OnEnable()
    {
        hitController.HitEvent += OnHit;
    }

    private void OnDisable()
    {
        hitController.HitEvent -= OnHit;
    }

    public void OnTry()
    {
        animCore.SetTrigger("Try");
    }

    public void OnActive()
    {
        animCore.SetTrigger("On");

        if (!(effectObj is null))
        {
            for (int i = 0; i < effectObj.Length; i++) effectObj[i].StartEffect();
            return;
        }
        if (!animObj) animObj.SetTrigger("On");
    }

    public void OnDeactive()
    {
        animCore.SetTrigger("Off");
    }

    private void OnHit()
    {
        UIManager.Alert("Core Object is under attack!!");
        animCore.SetTrigger("Hit");
    }
}
