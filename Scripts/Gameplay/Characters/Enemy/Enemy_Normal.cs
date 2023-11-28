using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy_Normal : Enemy_Movement_Normal
{
    public const float TIME_DELAY_DIE_ANIM = 2f;
    public const float DELAY_TIME_ANIM_ATTACK = 0.75f;
    public const float DELAY_TIME_ATTACK = 1.25f;
    public static readonly Vector3 HEALTH_BAR_POSITION = new Vector3(0f, 2.5f, 0f);
    public static readonly Vector3 CENTRAL_POSITION_OF_ENEMY = new Vector3(0f, 1.2f, 0f);


    void Start() 
    {
        OnInit();
    }

    
    private void Update() 
    {
        if(IsDead || !GameManager.Instance.IsState(GameState.GamePlay))
            return; 
        stateMachine?.Execute();
    }


    public override void OnInit()
    {
        offsetHealthBar = HEALTH_BAR_POSITION;
        base.OnInit();
    }

    protected override void LoadLevelData(ref float hp, ref float damage, ref float rangeAttack, ref float vision)
    {
        hp = level.levelData.normalEnemyHealth;
        damage = level.levelData.normalEnemyDamage;
        rangeAttack = level.levelData.normalEnemyAttackRange;
        vision = level.levelData.normalEnemyVision;
    }



    public override Vector3 GetPosition()
    {
       return TF.position + CENTRAL_POSITION_OF_ENEMY;
    }

    public override void OnDeath()
    {
        base.OnDeath();
        ChangeAnim(Constants.ANIM_DIE);
        Invoke(nameof(OnDespawn), TIME_DELAY_DIE_ANIM);
    }

    public override void Attack()
    {
        StartCoroutine(AttackCoroutine(player.GetPositon()));
    }

    IEnumerator AttackCoroutine(Vector3 target)
    {
        isAttacking = true;
        
        ChangeDirection(target);
        ChangeAnim(Constants.ANIM_ATTACK);
        
        Invoke(nameof(ActionAttack), DELAY_TIME_ANIM_ATTACK);
        yield return new WaitForSeconds(DELAY_TIME_ATTACK);
        isAttacking = false;
        DeActiveAttack();
        ResetAnim();
    }

    
}
