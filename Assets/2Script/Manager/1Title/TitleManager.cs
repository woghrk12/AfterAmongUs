using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private TitlePlayer titlePlayer = null;
    [SerializeField] private Transform[] spawnPositions = null;

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
        StartCoroutine(SpawnPlayer());
    }

    private IEnumerator SpawnPlayer()
    {
        var t_num = Random.Range(0, spawnPositions.Length);
        titlePlayer.transform.position = spawnPositions[t_num].position;
        titlePlayer.transform.localScale = new Vector3(t_num > 4 ? -1f : 1f, 1f, 1f);

        titlePlayer.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);
        
        titlePlayer.CanMove = true;
    }
}
