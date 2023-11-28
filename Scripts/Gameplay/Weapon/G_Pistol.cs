using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_Pistol : Weapon
{
    private void Awake() 
    {
        poolType = PoolType.G_Pistol;
    }

    public override WeaponType GetWeaponType()
    {
        return WeaponType.G_Pistol;
    }
}
