using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class HitEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle = null;
    [SerializeField] private Light2D pointLight = null;

    private void OnEnable()
    {
        StartCoroutine(PlayParticle());
    }

    private IEnumerator PlayParticle()
    {
        particle.Play();
        yield return ControllLight(particle.main.duration);
        ObjectPoolingManager.ReturnObject(gameObject);
    }

    private IEnumerator ControllLight(float p_duration)
    {
        var t_timer = 0f;
        
        while (t_timer < p_duration)
        {
            pointLight.intensity = Mathf.Lerp(1f, 0f, t_timer / p_duration);
            t_timer += Time.deltaTime;
            yield return null;
        }
    }
}
