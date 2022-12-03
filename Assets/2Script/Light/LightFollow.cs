using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFollow : MonoBehaviour
{
    [SerializeField] private Transform player = null;

    private void Update()
    {
        transform.position = player.position;
    }
}
