using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : AbCharacter, IHit
{
    [SerializeField] protected Animator anim;
    protected UIHealthBar healthBar;
    protected float hp;
    protected Vector3 offsetHealthBar;
    protected float rangeAttack;
    protected bool isAttacking;
    protected bool isMoving;
    public bool IsDead => hp <= 0;
    private string animName;

    public override void OnInit()
    {
        isAttacking = isMoving = false;
        
        // Create HealthBar
        CreateHealthBar();
        healthBar.OnInit(hp, TF, offsetHealthBar, this);


    }

    protected virtual void CreateHealthBar()
    {
        if(healthBar == null)
        {
            healthBar = SimplePool.Spawn<UIHealthBar>(PoolType.Health_Bar);
        }
    }

    public void ChangeAnim(string animName)
    {
        if (this.animName != animName)
        {
            anim.ResetTrigger(this.animName);
            this.animName = animName;
            anim.SetTrigger(this.animName);
        }
    }

    protected void ResetAnim()
    {
        animName = "";
    }

    protected void ChangeDirection(Vector3 target)
    {
        Vector3 lookDirection = target - TF.transform.position;
        Quaternion rotation = Quaternion.LookRotation(lookDirection, Vector3.up);

        TF.transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
    }

    public override void OnHit(float damage)
    {
        if(!IsDead)
        {
            hp -= damage;

            if(IsDead)
            {
                hp = 0;
                OnDeath();
            }
            if(healthBar != null)
            {
                healthBar.SetNewHP(hp);
            }
            
            SimplePool.Spawn<UICombatText>(PoolType.CombatText,TF.position + new Vector3(1f,0f,0f) + Vector3.up, Quaternion.identity).OnInit(damage);
        }
    }

    public override void OnDeath()
    {
        healthBar.OnDespawn();
    }

    public override void OnDespawn()
    {
        SimplePool.Despawn(this);
    }


}
