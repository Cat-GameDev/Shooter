using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICombatText : GameUnit
{
    [SerializeField] private TextMeshProUGUI hpText;

    public void OnInit(float damage)
    {
        hpText.text = damage.ToString();
        Invoke(nameof(OnDespawn), 1f);
    }

    public void OnDespawn()
    {
        SimplePool.Despawn(this);
    }
}
