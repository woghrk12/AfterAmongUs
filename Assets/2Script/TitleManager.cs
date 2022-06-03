using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private GameObject title;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameStart();
        }
    }

    private void GameStart()
    {
        title.GetComponent<Animator>().SetTrigger("FadeOut");
    }
}
