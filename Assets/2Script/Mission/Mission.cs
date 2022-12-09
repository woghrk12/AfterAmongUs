using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Mission : MonoBehaviour
{
    private Animator animCore = null;
    [SerializeField] private Region region = null;
    [SerializeField] private Animator animObj = null;
    [SerializeField] private int missionTime = 0;

    [SerializeField] private Damagable hitController = null;

    public Region Region { get { return region; } }
    public int MissionTime { get { return missionTime; } }
    public Damagable HitController { get { return hitController; } }

    private void Awake()
    {
        animCore = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        hitController.HitEvent += OnHit;
        hitController.DieEvent += OnDeactive;
    }

    private void OnDisable()
    {
        hitController.HitEvent -= OnHit;
        hitController.DieEvent -= OnDeactive;
    }

    public void OnTry()
    {
        animCore.SetTrigger("Try");
    }

    public void OnActive()
    {
        animCore.SetTrigger("On");
        animObj.SetTrigger("On");
    }

    public void OnDeactive()
    {
        animCore.SetTrigger("Off");
    }

    private void OnHit()
    {
        animCore.SetTrigger("Hit");
    }
}
