using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{
    public const string ANIM_RUN = "run";
    public const string ANIM_IDLE = "idle";
    public const string ANIM_SHOOT = "shoot";
    public const string ANIM_CHANGEWEAPON = "changeWeapon";
    public const string ANIM_DIE = "die";
    public const string ANIM_ATTACK = "attack";
    public const string ANIM_AWAKE = "awake";
    public const string ANIM_SPEEL= "speel";
}

public enum WeaponType
{
    G_Default = PoolType.G_Default,
    G_Pistol = PoolType.G_Pistol,

}