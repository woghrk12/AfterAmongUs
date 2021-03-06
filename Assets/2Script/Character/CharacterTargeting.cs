using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTargeting : MonoBehaviour
{
    private Transform target = null;
    public Transform Target { set { target = value; } get { return target; } }

    [SerializeField] private Transform playerAim = null;

    private float originAngle = 0f;
    private Vector3 direction = Vector3.zero;
    private float angle = 0f;

    private bool isTargeting = false;
    public bool IsTargeting
    {
        set 
        { 
            isTargeting = value; 
            if(!isTargeting) playerAim.rotation = Quaternion.AngleAxis(0f, Vector3.forward);
        }
        get { return isTargeting; }
    }

    private void Update()
    {
        Targeting(Target);
    }

    private void Targeting(Transform p_target)
    {
        if (!IsTargeting)
        {
            playerAim.rotation = Quaternion.AngleAxis(originAngle, Vector3.forward);
            return;
        }

        direction = p_target.position - playerAim.position;

        RotatePlayerAim(direction);
    }

    private void RotatePlayerAim(Vector3 p_dir)
    {
        var t_isLeft = p_dir.x < 0;
        
        transform.localScale = new Vector3(t_isLeft ? -1f : 1f, 1f, 1f);

        angle = (t_isLeft ? Mathf.Atan2(-p_dir.y, -p_dir.x) : Mathf.Atan2(p_dir.y, p_dir.x)) * Mathf.Rad2Deg;

        if (angle < -90f || angle > 90f)
            angle = Mathf.Clamp(angle, -90f, 90f);

        playerAim.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
