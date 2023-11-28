using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;

public class Enemy_Gun : Enemy_Movement_Normal
{
    public static readonly Vector3 HEALTH_BAR_POSITION = new Vector3(0f, 3.5f, 0f);
    public static readonly Vector3 CENTRAL_POSITION_OF_ENEMY = new Vector3(0f, 1.75f, 0f);
    public const float DELAY_TIME_ATTACK = 0.25f;
    public const float DELAY_TIME_ONDESPAWN = 1f;
    public const float DELAY_TIME_AWAKE = 0.7f;


    [SerializeField] Transform muzzleLeft;
    [SerializeField] Transform muzzleRight;



    private void Update() 
    {
        if(IsDead || !GameManager.Instance.IsState(GameState.GamePlay))
            return; 

        stateMachine?.Execute();
    }

    public override void OnInit()
    {
        vision = 9f;
        offsetHealthBar = HEALTH_BAR_POSITION;
        hp = 100f;
        base.OnInit();
    }

    protected override void LoadLevelData(ref float hp, ref float damage, ref float rangeAttack, ref float vision)
    {
        hp = level.levelData.gunEnemyHealth;
        damage = level.levelData.gunEnemyDamage;
        rangeAttack = level.levelData.gunEnemyAttackRange;
        vision = level.levelData.gunEnemyVision;
    }

    public override Vector3 GetPosition()
    {
       return TF.position + CENTRAL_POSITION_OF_ENEMY;
    }

    public override void OnDeath()
    {
        base.OnDeath();
        ChangeAnim(Constants.ANIM_DIE);
        Invoke(nameof(OnDespawn), DELAY_TIME_ONDESPAWN); 
        // explode particle
        ExpoldeParticle();
    }

    private void ExpoldeParticle()
    {
        ParticlePool.Play(ParticleType.Enemy_Gun_Explosion_Particle, GetPosition(), Quaternion.identity);
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
        
        Shoot(target + player.GetCentralPosition(), damage);
        yield return new WaitForSeconds(DELAY_TIME_ATTACK);
        isAttacking = false;
        ResetAnim();
    }

    private void Shoot(Vector3 target, float damage)
    {
        Bullet bullet1 = SimplePool.Spawn<Bullet>(PoolType.Bullet_Enemy, muzzleLeft.position, Quaternion.identity);
        Bullet bullet2 = SimplePool.Spawn<Bullet>(PoolType.Bullet_Enemy, muzzleRight.position, Quaternion.identity);
        bullet1.OnInit(target, damage);
        bullet1.TF.localScale = Vector3.one;
        bullet2.OnInit(target, damage);
        bullet2.TF.localScale = Vector3.one;
    }

    

    private void ConvertAttackState()
    {
        stateMachine.ChangeState(AttackState);
    }



    protected override void IdleState(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {

        onEnter = () =>
        {

        };

        onExecute = () =>
        {
            if(PlayerIsInRange(vision))
            {
                ChangeAnim(Constants.ANIM_AWAKE);
                Invoke(nameof(ConvertAttackState), DELAY_TIME_AWAKE);
            }
        };

        onExit = () =>
        {
    
        };
    }  


}
