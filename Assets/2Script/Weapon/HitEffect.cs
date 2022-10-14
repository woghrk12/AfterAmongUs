using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle = null;

    private void OnEnable()
    {
        StartCoroutine(PlayParticle());
    }

    private IEnumerator PlayParticle()
    {
        particle.Play();
        yield return new WaitForSeconds(particle.main.duration);
        ObjectPoolingManager.ReturnObject(gameObject);
    }
}
