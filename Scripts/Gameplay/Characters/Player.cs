using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : Character
{
    public const float TIME_DELAY_DIE_ANIM = 1.5f;
    public static readonly Vector3 HEALTH_BAR_POSITION = new Vector3(0f, 3.2f, 0f);
    public static readonly Vector3 CENTRAL_POSITON_OF_PLAYER = new Vector3(0f, 1.5f, 0f); 
    float moveSpeed;
    protected float timeRate;
    [SerializeField] PlayerData playerData;
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform rightHand;
    [SerializeField] WeaponData weaponData;
    
    [SerializeField] List<Enemy> enemies = new List<Enemy>();
    Weapon currentWeapon;
    bool isChangeWeapon = false;
    bool isAlive = false;


    void Update()
    {
        if (GameManager.Instance.IsState(GameState.GamePlay) )
        {
            if(isChangeWeapon || IsDead)
                return;

            if (JoystickControl.direct != Vector3.zero)
            {
                isMoving = true;
                Moving();
            }
            else
            {
                isMoving = false;
            }

            if(!isMoving && !isAttacking)
            {
                AutoShootEnemy();
            }
        }
    }

    public override void OnInit()
    {
        LoadPlayerData(ref moveSpeed, ref rangeAttack, ref hp);
        gameObject.SetActive(true);
        TF.position = LevelManager.Instance.StartPoint.position;
        offsetHealthBar = HEALTH_BAR_POSITION;

        base.OnInit();
        isChangeWeapon = false;
        ChangeWeapon();
    }

    private void LoadPlayerData(ref float moveSpeed,ref float rangeAttack,ref float hp)
    {
        moveSpeed = playerData.MoveSpeed;
        rangeAttack = playerData.RangeAttack;
        if(!isAlive)
        {
            hp = playerData.Hp;
            isAlive = true;
        }
       
    }
    
    private void AutoShootEnemy()
    {
        enemies = LevelManager.Instance.Enemies;
        Enemy enemyTarget = null;
        float minDistance = float.MaxValue;

        // find enemy closest player
        for(int i =0; i<enemies.Count; i++)
        {
            float distance = Vector3.Distance(TF.position, enemies[i].GetPosition());
            if(distance < rangeAttack) // tam danh Player
            {
                if(distance < minDistance)
                {
                    minDistance = distance;
                    enemyTarget = enemies[i];
                }
            }
            
        }

        if (enemyTarget != null)
        {
            StartCoroutine(AttackCoroutine(enemyTarget.GetPosition())); 
        }
        else
        {
            Idle();
        }
    }

    private void Moving()
    {
        ChangeAnim(Constants.ANIM_RUN);
        rb.MovePosition(rb.position + JoystickControl.direct * moveSpeed * Time.deltaTime);
        TF.position = rb.position;
        TF.forward = JoystickControl.direct;
    }

    private void Idle()
    {
        ChangeAnim(Constants.ANIM_IDLE);
    }

    public void StartChangeWeapon()
    {
        isChangeWeapon = true;
        ChangeAnim(Constants.ANIM_CHANGEWEAPON);
        Invoke(nameof(ChangeWeapon), .5f);
    }


    public void ChangeWeapon()
    {
        currentWeapon = weaponData.ChangeWeapon(currentWeapon, rightHand, anim);
        currentWeapon.OnInit(currentWeapon.GetWeaponType());
        isChangeWeapon = false;
        ResetAnim();
    }


    IEnumerator AttackCoroutine(Vector3 tagret)
    {
        isAttacking = true; 
        
        ChangeDirection(tagret);
        ChangeAnim(Constants.ANIM_SHOOT);


        currentWeapon.Shoot(tagret,ref timeRate);
        yield return new WaitForSeconds(timeRate);

        isAttacking = false;
        if(!isMoving)
        {
            ResetAnim();
        } 
    }

    public override void OnDeath()
    {
        isAlive = false;
        base.OnDeath();
        ChangeAnim(Constants.ANIM_DIE);
        Invoke(nameof(DeActivePlayer), TIME_DELAY_DIE_ANIM);
        LevelManager.Instance.Fall();
        healthBar = null;
    }

    private void DeActivePlayer()
    {
        gameObject.SetActive(false);
    }

    public Vector3 GetCentralPosition()
    {
        return CENTRAL_POSITON_OF_PLAYER;
    }

    public Vector3 GetPositon()
    {
        return TF.position;
    }




}
