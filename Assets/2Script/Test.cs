using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private CharacterRader rader = null;

    private void Awake()
    {
        rader.SetRange(2f);
    }
}
