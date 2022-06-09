using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private GameObject titleImage;

    [SerializeField] private Button gameStartButton;

    [SerializeField] private CinemachineVirtualCamera startVirtualCamera;
    [SerializeField] private CinemachineVirtualCamera playerVirtualCamera;

    [SerializeField] private Transform[] spawnPositions;

    [SerializeField] private TitlePlayer player;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            SwitchCamera(true);
        if (Input.GetKeyDown(KeyCode.R))
            SwitchCamera(false);
    }

    private void SwitchCamera(bool p_isPlayer)
    {
        playerVirtualCamera.gameObject.SetActive(p_isPlayer);
        startVirtualCamera.gameObject.SetActive(!p_isPlayer);
    }

    private IEnumerator GameStart()
    {
        gameStartButton.GetComponent<Animator>().SetTrigger("FadeOut");
        gameStartButton.enabled = false;

        yield return new WaitForSeconds(0.5f);

        titleImage.GetComponent<Animator>().SetTrigger("FadeOut");

        yield return new WaitForSeconds(0.5f);
       
        var t_randomNum = Random.Range(0, spawnPositions.Length);
        var t_spawnPosition = spawnPositions[t_randomNum];

        player.transform.position = t_spawnPosition.position;

        SwitchCamera(true);

        yield return new WaitForSeconds(2f);
        
        player.SpawnPlayer(t_randomNum >= (spawnPositions.Length / 2));

        yield return new WaitForSeconds(0.5f);

        player.canMove = true;
    }

    public void OnClickStartButton()
    {
        StartCoroutine(GameStart());
    }
}
