using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData", order = 1)]
public class WeaponData : ScriptableObject
{
    [SerializeField] List<WeaponItem> weaponItems = new List<WeaponItem>();
    int currentWeaponIndex = 0;
    private void Start() 
    {
        currentWeaponIndex = 0;
    }

    public WeaponItem GetWeaponItem(WeaponType weaponType)
    {
        return weaponItems.Single(q => q.type == weaponType);
    }


    public Weapon ChangeWeapon(Weapon currentWeapon, Transform spawnWeaponPosition, Animator anim)
    {
        if(currentWeapon != null)
        {
            SimplePool.Despawn(currentWeapon);
        }

        WeaponItem nextWeaponItem = weaponItems[currentWeaponIndex];
        currentWeapon = SimplePool.Spawn<Weapon>((PoolType)nextWeaponItem.type, spawnWeaponPosition);
        ChangeAnimator(currentWeapon, anim);
        currentWeaponIndex = (currentWeaponIndex + 1) % weaponItems.Count;
        return currentWeapon;
    }

    private void ChangeAnimator(Weapon currentWeapon, Animator anim)
    {
        anim.runtimeAnimatorController = currentWeapon.AnimatorOverrideController;
    }

    public float GetWeaponDamage(WeaponType weaponType)
    {
        WeaponItem weaponItem = GetWeaponItem(weaponType);

        if (weaponItem != null)
        {
            return weaponItem.damage;
        }

        Debug.LogWarning($"Weapon type {weaponType} not found (damage).");
        return 0f;
    }

    public float GetFireRate(WeaponType weaponType)
    {
        WeaponItem weaponItem = GetWeaponItem(weaponType);

        if (weaponItem != null)
        {
            return weaponItem.fireRate;
        }

        Debug.LogWarning($"Weapon type {weaponType} not  (fireRate).");
        return 0f;
    }


}   

[System.Serializable]
public class WeaponItem
{
    public string name;
    public WeaponType type;
    public float damage;
    public float fireRate;
    public int cost;
    public int ads;
}


