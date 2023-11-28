using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : GameUnit
{
    [SerializeField] WeaponData weaponData;
    [SerializeField] protected AnimatorOverrideController animatorOverrideController;
    [SerializeField] protected Transform muzzle;
    float fireRate;
    float damage;
    public AnimatorOverrideController AnimatorOverrideController { get => animatorOverrideController;}

    public void OnInit(WeaponType weaponType)
    {
        damage = weaponData.GetWeaponDamage(weaponType);
        fireRate = weaponData.GetFireRate(weaponType);
    }

    public virtual void Shoot(Vector3 target, ref float timeRate)
    {
        Bullet bullet = SimplePool.Spawn<Bullet>(PoolType.Bullet_Player, muzzle.position, Quaternion.identity);
        bullet.OnInit(target, damage);
        bullet.TF.localScale = Vector3.one;
        timeRate = this.fireRate;
    }

   public abstract WeaponType GetWeaponType();

    

    
}
