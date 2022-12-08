using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    private UIManager manager = null;
    private TitleUIGroup titleUI = null;
    private FadeUIGroup fadeUI = null;

    [SerializeField] private TitlePlayer titlePlayer = null;
    [SerializeField] private Transform[] spawnPositions = null;

    private void Start()
    {
        manager = UIManager.Instance;
        titleUI = manager.TitleUI;
        fadeUI = manager.FadeUI;

        StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
        Camera.main.GetComponent<CameraShaking>().StartShaking(15f);
        
        manager.ActiveUI(EUIList.FADE);
        fadeUI.InitUI();

        yield return fadeUI.FadeIn();
        
        manager.ActiveUI(EUIList.TITLE);
        titleUI.SetControl();
        titleUI.InitUI();
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

        yield return Utilities.WaitForSeconds(0.5f);

        titlePlayer.gameObject.SetActive(true);
        titlePlayer.InitPlayer();

        yield return Utilities.WaitForSeconds(1f);
        
        titlePlayer.CanMove = true;
    }

    public void EnterInGame() => StartCoroutine(EnterInGameCo());

    private IEnumerator EnterInGameCo()
    {
        titlePlayer.CanMove = false;
        manager.ActiveUI(EUIList.FADE);
        StartCoroutine(fadeUI.FadeOut());
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
