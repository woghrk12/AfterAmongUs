using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class TitleManager : MonoBehaviour
{
    public static TitleManager instance;

    [SerializeField] private GameObject titleImage;
    [SerializeField] private Image background;

    [SerializeField] private Button startButton;
    [SerializeField] private Button gameStartButton;

    [SerializeField] private CinemachineVirtualCamera startVirtualCamera;
    [SerializeField] private CinemachineVirtualCamera playerVirtualCamera;

    [SerializeField] private Transform[] spawnPositions;

    [SerializeField] private TitlePlayer player;
    [SerializeField] private GameObject playerUI;
    [SerializeField] private ColorSelectPanel colorSelectPanel;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetPlayerColor(GameManager.playerColor);
    }

    private void SwitchCamera(bool p_isPlayer)
    {
        playerVirtualCamera.gameObject.SetActive(p_isPlayer);
        startVirtualCamera.gameObject.SetActive(!p_isPlayer);
    }

    public void OnClickStartButton()
    {
        StartCoroutine(GameStart());
    }

    private IEnumerator GameStart()
    {
        startButton.GetComponent<Animator>().SetTrigger("FadeOut");
        startButton.enabled = false;

        yield return new WaitForSeconds(0.5f);

        titleImage.GetComponent<Animator>().SetTrigger("FadeOut");

        yield return new WaitForSeconds(0.5f);
       
        var t_randomNum = Random.Range(0, spawnPositions.Length);
        var t_spawnPosition = spawnPositions[t_randomNum];

        player.transform.position = t_spawnPosition.position;

        SwitchCamera(true);

        yield return new WaitForSeconds(2f);

        playerUI.SetActive(true);
        player.SpawnPlayer(t_randomNum >= (spawnPositions.Length / 2));

        yield return new WaitForSeconds(0.5f);

        player.CanMove = true;
    }

    public void OnClickGameStartButton()
    {
        StartCoroutine(SwitchToInGameScene());
    }

    private IEnumerator SwitchToInGameScene()
    {
        player.CanMove = false;
        gameStartButton.interactable = false;
        SwitchCamera(false);

        yield return new WaitForSeconds(2f);

        playerUI.SetActive(false);

        var t_totalTime = 0.5f;
        var t_timer = 0f;
        var t_position= startVirtualCamera.transform.position;

        while (t_timer <= t_totalTime)
        {
            t_position.y = Mathf.Lerp(0f, 5f, t_timer / t_totalTime);
            startVirtualCamera.transform.position = t_position;
            t_timer += Time.deltaTime;
            yield return null;
        }

        t_timer = 0f;

        while (t_timer <= t_totalTime)
        {
            t_position.y = Mathf.Lerp(5f, -15f, t_timer / t_totalTime);
            startVirtualCamera.transform.position = t_position;
            t_timer += Time.deltaTime;
            yield return null;
        }

        t_timer = 0f;
        background.gameObject.SetActive(true);

        yield return ChangeBackground(1f);

        LoadingManager.LoadScene(EScene.INGAME);
    }

    private IEnumerator ChangeBackground(float p_time)
    {
        var t_timer = 0f;
        var t_color = background.color;
        var t_fadeIn = t_color.a == 1f;
        while (t_timer <= p_time)
        {
            t_color.a = Mathf.Lerp(t_fadeIn ? 1f : 0f, t_fadeIn ? 0f : 1f, t_timer / p_time);
            background.color = t_color;
            t_timer += Time.deltaTime;
            yield return null;
        }

        t_color.a = t_fadeIn ? 0f : 1f;
        background.color = t_color;
    }

    public static void TurnOnColorSelectPanel() => instance.EnableColorSelectPanel();
    public static void TurnOffColorSelectPanel() => instance.DisableColorSelectPanel();

    private void EnableColorSelectPanel()
    {
        player.CanMove = false;
        playerUI.SetActive(false);
        colorSelectPanel.EnablePanel();
    }

    private void DisableColorSelectPanel()
    {
        player.CanMove = true;
        playerUI.SetActive(true);
        colorSelectPanel.DisablePanel();
    }

    public static void ChangePlayerColor(EPlayerColor p_color) => instance.SetPlayerColor(p_color);

    private void SetPlayerColor(EPlayerColor p_color) => player.SetColor(p_color);
}
