using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject 
{
   
    [Header("Player")]
    public Vector3 playerStartPoint; 

    [Header("Enemy Normal")]
    public int normalEnemyAmount;
    public float normalEnemyHealth = 100f; 
    public float normalEnemyDamage = 20f;
    public float normalEnemyAttackRange = 2.3f;
    public float normalEnemyVision = 9f;

    [Header("Enemy Explode")]
    public int explodeEnemyAmount;
    public float explodeEnemyHealth = 150f; 
    public float explodeEnemyDamge = 30f;
    public float explodeEnemyAttackRange = 1.5f;
    public float explodeEnemyVision = 9f;

    [Header("Enemy Gun")]
    public int gunEnemyAmount;
    public float gunEnemyHealth = 200f; 
    public float gunEnemyDamage = 20f;
    public float gunEnemyAttackRange = 9f;
    public float gunEnemyVision = 9f;

    [Header("Enemy Launch")]
    public int launchEnemyAmount;
    public float launchEnemyHealth = 150f; 
    public float launchEnemyDamage = 20f;
    public float launchEnemyAttackRange = 4f;
    public float launchEnemyVision = 9f;

    [Header("Attack Area")]
    public Vector3 patrolAreaCenter1; 
    public Vector3 patrolAreaSize1;

    public int[] enemiesPerRound = { 1, 1, 1 }; 
    public PoolType[] enemyTypesPerRoundArray;


}
