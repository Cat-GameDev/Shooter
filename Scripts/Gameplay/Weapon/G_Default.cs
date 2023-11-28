using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_Default : Weapon
{
    private void Awake() 
    {
        poolType = PoolType.G_Default;
    }

    public override WeaponType GetWeaponType()
    {
        return WeaponType.G_Default;
    }
}
