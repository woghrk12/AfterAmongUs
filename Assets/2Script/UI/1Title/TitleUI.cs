using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    private TitleManager titleManager = null;
    [SerializeField] private List<UIBase> uiList = null;
    private ETitleUILayer activeUI = ETitleUILayer.END;

    [SerializeField] private JoyStick joyStick = null;
    [SerializeField] private Button useButton = null;

    public JoyStick JoyStick { get { return joyStick; } }
    public Button UseButton { get { return useButton; } }

    public void InitUI()
    {
        titleManager = FindObjectOfType<TitleManager>();

        if (!GameManager.isStart)
        {
            StartCoroutine(ControlUI(true, ETitleUILayer.START, true));
            GameManager.isStart = true;
        }
        else
            StartCoroutine(ControlUI(true, ETitleUILayer.SHIP));
    }

    private IEnumerator ControlUI(bool p_isOn, ETitleUILayer p_layer = ETitleUILayer.END, bool p_isEffect = false)
    {
        if (!activeUI.Equals(ETitleUILayer.END)) yield return uiList[(int)activeUI].OffUI(true);
        if (!p_isOn) yield break;

        activeUI = p_layer;
        yield return uiList[(int)p_layer].OnUI(p_isEffect);
    }

    #region OnClick Function

    public void OnClickGameStartButton() => StartCoroutine(EnterShip());
    public void OnClickInGameButton()
    {
        if (!titleManager.CheckCanStartGame()) return;
        StartCoroutine(EnterGame());
    }
    public void OnClickBackground() => StartCoroutine(ControlUI(true, ETitleUILayer.SHIP));
    public void OnClickUseButton() => StartCoroutine(ControlUI(true, ETitleUILayer.CUSTOM, true));

    #endregion

    private IEnumerator EnterShip()
    {
        yield return ControlUI(true, ETitleUILayer.SHIP, true);
        titleManager.GameStart();
    }

    private IEnumerator EnterGame()
    {
        yield return ControlUI(false);
        titleManager.EnterInGame();
    }
}
