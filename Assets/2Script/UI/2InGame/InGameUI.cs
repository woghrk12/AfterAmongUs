using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    private GamePlayer gamePlayer = null;
    [SerializeField] private ControlUI controlUI = null;
    [SerializeField] private WeaponUI weaponUI = null;
    [SerializeField] private StatusUI statusUI = null;

    public JoyStick JoyStick { get { return controlUI.JoyStick; } }
    public Button UseButton { get { return controlUI.UseButton; } }

    public void InitUI()
    {
        gamePlayer = FindObjectOfType<GamePlayer>();
        weaponUI.InitUI(GameManager.playerWeapon);
        statusUI.InitUI(gamePlayer.EquipWeapon);
    }

    #region OnClick Function

    public void OnClickReloadButton() => StartCoroutine(PlayerReload());

    public void OnClickSwapButton(int p_idx)
    {
        if (gamePlayer.IsReload) return;
        
        var t_weaponInfo = gamePlayer.Swap(p_idx);

        statusUI.SetMaxBullet(t_weaponInfo.Item1);
        statusUI.SetBullet(t_weaponInfo.Item2);
        weaponUI.SetSwapButton(p_idx);
    }

    public void OnClickUseButton(IInteractable p_interactable)
    {
        p_interactable.Use();
    }

    #endregion

    private IEnumerator PlayerReload()
    {
        weaponUI.SetReloadButton(false);
        yield return gamePlayer.Reload();
        weaponUI.SetReloadButton(true);
    }
}
