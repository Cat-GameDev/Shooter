using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : UICanvas
{
    public void ChangeWeaponButton()
    {
        Player player = LevelManager.Instance.Player;
        player.StartChangeWeapon();
    }

    public void SettingButton()
    {
        UIManager.Instance.OpenUI<Settings>();
    }
}
