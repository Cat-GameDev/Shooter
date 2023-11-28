using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Launch : Enemy_Movement_Normal
{
    public const float TIME_DELAY_DIE_ANIM = 1.5f;
    public const float DELAY_TIME_ATTACK = 1f;
    public static readonly Vector3 HEALTH_BAR_POSITION = new Vector3(0f, 1.5f, 0f);
    public static readonly Vector3 CENTRAL_POSITION_OF_ENEMY = new Vector3(0f, 0.35f, 0f);

    [SerializeField] Transform muzzle;

    
    private void Update() 
    {
        if(IsDead || !GameManager.Instance.IsState(GameState.GamePlay))
            return; 
        stateMachine?.Execute();
    }


    public override void OnInit()
    {
        vision = 5f;
        offsetHealthBar = HEALTH_BAR_POSITION;
        hp = 100f;
        base.OnInit();
    }

    protected override void LoadLevelData(ref float hp, ref float damage, ref float rangeAttack, ref float vision)
    {
        hp = level.levelData.launchEnemyHealth;
        damage = level.levelData.launchEnemyDamage;
        rangeAttack = level.levelData.launchEnemyAttackRange;
        vision = level.levelData.launchEnemyVision;
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
        // shoot
        Shoot(target);
        yield return new WaitForSeconds(DELAY_TIME_ATTACK);
        isAttacking = false;
        ResetAnim();
    }


    private void Shoot(Vector3 target)
    {
        Projectile_Launch projectile = SimplePool.Spawn<Projectile_Launch>(PoolType.Projectile_Launch, muzzle.position, Quaternion.identity);
        projectile.Launch(target, damage);
        projectile.TF.localScale = Vector3.one;
    }
}
