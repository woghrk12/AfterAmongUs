using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private TitlePlayer titlePlayer = null;

    private void Awake()
    {
        titlePlayer = FindObjectOfType<TitlePlayer>();
    }

    private void Start()
    {
        titlePlayer.gameObject.SetActive(false);
    }

    public void GameStart()
    {
        titlePlayer.gameObject.SetActive(true);
    }
}
