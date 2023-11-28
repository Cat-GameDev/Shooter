using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class Enemy_Movement_Normal : Enemy
{
    public override void Attack(){}
    public override Vector3 GetPosition() { return Vector3.zero; }
    protected override void IdleState(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        float randonTime = Random.Range(1f, 1.5f);
        float idleTimer = 0f;
        onEnter = () =>
        {
            idleTimer = 0f;
            StopMoving();
        };

        onExecute = () =>
        {
            idleTimer += Time.deltaTime;
            if(idleTimer > randonTime)
            {
                stateMachine.ChangeState(PatrolState);
            }
        };

        onExit = () =>
        {
            
        };
    }  

    protected override void PatrolState(ref Action onEnter, ref Action onExecute, ref Action onExit) 
    {
        
        Vector3 patrolDestination = Vector3.zero;
        float randonTime = Random.Range(2f, 3f);
        float idleTimer = 0f;

        onEnter = () =>
        {
            idleTimer = 0f;
            patrolDestination = GetRandomPointInPatrolArea(patrolAreaCenter, patrolAreaSize);
        };

        onExecute = () =>
        {
            if(PlayerIsInRange(vision))
            {
                stateMachine.ChangeState(AttackState);
            }
            else
            {
                idleTimer += Time.deltaTime;
                if(idleTimer > randonTime)
                {
                    stateMachine.ChangeState(IdleState);
                }
                else
                {
                    if (navMeshAgent.remainingDistance < 0.1f)
                    {
                        patrolDestination = GetRandomPointInPatrolArea(patrolAreaCenter, patrolAreaSize);
                    }
                    Moving(patrolDestination);
                }
                
            }
        };

        onExit = () =>
        {
            
        };

    }

    protected override void AttackState(ref Action onEnter, ref Action onExecute, ref Action onExit) 
    {

        onExecute = () =>
        {
            if(IsDead || isAttacking)
                return;
            
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
