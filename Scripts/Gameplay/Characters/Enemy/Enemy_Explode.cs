using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy_Explode : Enemy_Movement_Normal
{
    public const float TIME_SELF_DESTROY = 4f;
    public static readonly Vector3 HEALTH_BAR_POSITION = new Vector3(0f, 1.5f, 0f);
    public static readonly Vector3 CENTRAL_POSITION_OF_ENEMY = new Vector3(0f, 0.5f, 0f);
    bool isExplode = false;


    private void Update() 
    {
        if(IsDead || !GameManager.Instance.IsState(GameState.GamePlay))
            return; 

        stateMachine?.Execute();
    }


    public override void OnInit()
    {
        isExplode = false;
        vision = 5f;
        offsetHealthBar = HEALTH_BAR_POSITION;
        hp = 100f;
        base.OnInit();
    }

    protected override void LoadLevelData(ref float hp, ref float damage, ref float rangeAttack, ref float vision)
    {
        hp = level.levelData.explodeEnemyHealth;
        damage = level.levelData.explodeEnemyDamge;
        rangeAttack = level.levelData.explodeEnemyAttackRange;
        vision = level.levelData.explodeEnemyVision;
    }

    public override Vector3 GetPosition()
    {
       return TF.position + CENTRAL_POSITION_OF_ENEMY;
    }

    public override void OnDeath()
    {
        if(!IsDead)
        {
            hp = 0;
            LevelManager.Instance.RemoveEnemyFromList(this);
        }

        if(!isExplode)
        {
            ParticlePool.Play(ParticleType.Enemy_Explode_Particle, TF.position + new Vector3(0f,1f,0f), Quaternion.identity);
            isExplode = true;
        }
        
        base.OnDeath();
        OnDespawn();
    }

    protected override void Moving(Vector3 target)
    {
        ChangeAnim(Constants.ANIM_RUN);
        navMeshAgent.SetDestination(target);
    }

    public override void Attack()
    {
        //rangeAttack = 1.5f;

        if (!isAttacking)
        {
            navMeshAgent.speed = 6f;
            Invoke(nameof(OnDeath), TIME_SELF_DESTROY);
            isAttacking = true; 
        }

    }

    protected override void AttackState(ref Action onEnter, ref Action onExecute, ref Action onExit) 
    {
        onExecute = () =>
        {
            if(PlayerIsInRange(rangeAttack))
            {
                StopMoving();
                Attack();
            }
            else 
            {
                Moving(player.GetPositon());
            }
        };


    }


}
