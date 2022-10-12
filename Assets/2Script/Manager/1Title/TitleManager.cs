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
        titlePlayer.SetPlayerColor((int)GameManager.playerColor);
        titlePlayer.gameObject.SetActive(false);
        Camera.main.GetComponent<CameraShaking>().StartShaking(15f);
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

        yield return new WaitForSeconds(0.5f);

        titlePlayer.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);
        
        titlePlayer.CanMove = true;
    }

    public void EnterInGame() => StartCoroutine(EnterInGameCo());

    private IEnumerator EnterInGameCo()
    {
        titlePlayer.CanMove = false;
        yield return new WaitForSeconds(2f);
        LoadingManager.LoadScene(EScene.INGAME);
    }

    public bool CheckCanStartGame()
    {
        if (!CheckWeapon(GameManager.playerWeapon))
        {
            UIManager.Alert("We need two weapons!!");
            return false;
        }

        return true;
    }

    private bool CheckWeapon(EPlayerWeapon[] p_weapons)
    {
        for (int i = 0; i < p_weapons.Length; i++)
            if (p_weapons[i] == EPlayerWeapon.NONE) return false;

        return true;
    }
}
