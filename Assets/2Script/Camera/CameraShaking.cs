using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaking : MonoBehaviour
{
    [SerializeField] private float force = 0f;
    [SerializeField] private Vector3 offset = Vector3.zero;

    private Quaternion originRot = Quaternion.identity;

    private Coroutine shakeCo = null;
    private Coroutine reduceCo = null;

    public bool ShakeSwitch
    {
        set 
        {
            if (value)
            {
                if (!(shakeCo is null)) return;
                shakeCo = StartCoroutine(ShakeCameraCo());
            }
            else
            {
                if (shakeCo is null) return;
                StopCoroutine(shakeCo);
                shakeCo = null;
            }
        }
    }

    private void Awake()
    {
        originRot = transform.rotation;
    }

    private IEnumerator ShakeCameraCo()
    {
        var t_originEuler = originRot.eulerAngles;

        while (true)
        {
            var t_randomRot = t_originEuler + new Vector3(
                    Random.Range(-offset.x, offset.x),
                    Random.Range(-offset.y, offset.y),
                    Random.Range(-offset.z, offset.z)
                    );
            var t_rot = Quaternion.Euler(t_randomRot);

            while (Quaternion.Angle(transform.rotation, t_rot) > 0.1f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, t_rot, force * Time.deltaTime);
                yield return null;
            }
            yield return null;
        }
    }

    public void StartShaking(float p_force)
    {
        force = p_force;
        ShakeSwitch = true;
    }

    public void ShakeCamera(float p_force, float p_time)
    {
        if (!(reduceCo is null)) StopCoroutine(reduceCo);

        force = p_force;
        ShakeSwitch = true;
        reduceCo = StartCoroutine(ReduceForce(p_time));
    }

    private IEnumerator ReduceForce(float p_totalTime)
    {
        var t_timer = 0f;
        var t_originForce = force;

        while (t_timer < p_totalTime)
        {
            force = Mathf.Lerp(t_originForce, 0f, t_timer / p_totalTime);
            t_timer += Time.deltaTime;
            yield return null;
        }

        ShakeSwitch = false;
        transform.rotation = originRot;
        force = t_originForce;
    }
}
