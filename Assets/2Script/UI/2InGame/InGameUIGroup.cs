using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIGroup : UIGroup
{
    private InGameManager manager = null;
    private GamePlayer gamePlayer = null;

    [SerializeField] private ControlUI controlUI = null;
    [SerializeField] private WeaponUI weaponUI = null;
    [SerializeField] private StatusUI statusUI = null;
    [SerializeField] private Text timerText = null;

    public JoyStick JoyStick { get { return controlUI.JoyStick; } }
    public Button UseButton { get { return controlUI.UseButton; } }
    public StatusUI StatusUI { get { return statusUI; } }

    public override void InitUI()
    {
        manager = FindObjectOfType<InGameManager>();
        gamePlayer = manager.GamePlayer;
        weaponUI.InitUI(GameManager.playerWeapon);
        statusUI.InitUI(gamePlayer.EquipWeapon);
    }

    public void SetControl()
    {
        UIManager.Instance.Joystick = JoyStick;
        UIManager.Instance.UseButton = UseButton;
    }

    #region OnClick Function

    public void OnClickReloadButton() => StartCoroutine(PlayerReload());

    public void OnClickSwapButton(int p_idx)
    {
        if (gamePlayer.IsReload) return;

        gamePlayer.Swap(p_idx, statusUI.BulletStatus);
        weaponUI.SetSwapButton(p_idx);
    }

    public void OnClickUseButton(IInteractable p_interactable)
    {
        p_interactable.Use();
    }

    #endregion

    public void PlayerFire(bool p_isFire) { gamePlayer.IsFire = p_isFire; }

    private IEnumerator PlayerReload()
    {
        weaponUI.SetReloadButton(false);
        gamePlayer.IsReload = true;
        yield return gamePlayer.Reload(statusUI.BulletStatus);
        gamePlayer.IsReload = false;
        weaponUI.SetReloadButton(true);
    }

    public void InitTimer(int p_initValue)
    {
        if (!timerText.gameObject.activeSelf) timerText.gameObject.SetActive(true);
        timerText.text = p_initValue.ToString();
        timerText.color = Color.white;
    }

    public void ShowTimer(int p_time)
    {
        timerText.text = p_time.ToString();
        if (p_time <= 10) timerText.color = Color.red;
    }

    public void EndTimer()
    {
        timerText.gameObject.SetActive(false);
    }
}
