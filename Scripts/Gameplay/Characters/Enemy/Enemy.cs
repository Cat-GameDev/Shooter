using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public abstract class Enemy : Character
{
    protected StateMachine stateMachine = new StateMachine();
    [SerializeField] protected NavMeshAgent navMeshAgent; 
    public Vector3 patrolAreaCenter; 
    public Vector3 patrolAreaSize;
    [SerializeField] AttackArea attackArea;
    protected float damage;
    protected float vision;
    protected Player player;
    protected Level level;

    public float Damage { get => damage;  }

    public abstract void Attack();
    public abstract Vector3 GetPosition();



    public override void OnInit()
    {
        level = LevelManager.Instance.levels[LevelManager.Instance.LevelIndex];
        level.UpdateArea(ref patrolAreaCenter, ref patrolAreaSize);
        base.OnInit();
        stateMachine.ChangeState(IdleState);
        player = LevelManager.Instance.Player;
        LoadLevelData(ref hp, ref damage, ref rangeAttack, ref vision);
        
    }

    protected virtual void LoadLevelData(ref float hp, ref float damage, ref float rangeAttack, ref float vision)
    {

    }

    protected void ActionAttack()
    {
        attackArea.gameObject.SetActive(true);
        
    }

    protected void DeActiveAttack()
    {
        attackArea.gameObject.SetActive(false);
    }

    
    public override void OnHit(float damage)
    {
        base.OnHit(damage);
        if(IsDead)
        {
            LevelManager.Instance.RemoveEnemyFromList(this);
        }
        
    }

    public override void OnDeath()
    {
        base.OnDeath();
        ResetNavmesh();
    }

    public void ResetNavmesh()
    {
        navMeshAgent.ResetPath();
    }

    protected virtual void Moving(Vector3 target)
    {
        if(IsDead || isAttacking)
            return;
        
        ChangeAnim(Constants.ANIM_RUN);
        navMeshAgent.SetDestination(target);
    }
    
    public virtual void StopMoving()
    {

        navMeshAgent.ResetPath();
        ChangeAnim(Constants.ANIM_IDLE);
    }

    protected bool PlayerIsInRange(float rangeAttack)
    {
        float distance = Vector3.Distance(player.TF.position, TF.position);

        if(distance < rangeAttack)
        {
            return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(patrolAreaCenter, patrolAreaSize);
    }

    public Vector3 GetRandomPointInPatrolArea(Vector3 patrolAreaCenter, Vector3 patrolAreaSize)
    {
        float randomX = Random.Range(patrolAreaCenter.x - patrolAreaSize.x / 2, patrolAreaCenter.x + patrolAreaSize.x / 2);
        float randomZ = Random.Range(patrolAreaCenter.z - patrolAreaSize.z / 2, patrolAreaCenter.z + patrolAreaSize.z / 2);

        Vector3 randomPoint = new Vector3(randomX, 0f, randomZ);
        return randomPoint;
    }


    protected abstract void IdleState(ref Action onEnter, ref Action onExecute, ref Action onExit);

    protected abstract void PatrolState(ref Action onEnter, ref Action onExecute, ref Action onExit);

    protected abstract void AttackState(ref Action onEnter, ref Action onExecute, ref Action onExit);


}
