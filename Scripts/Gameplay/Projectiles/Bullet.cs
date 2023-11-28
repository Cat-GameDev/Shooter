using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : GameUnit
{
    [SerializeField] float moveSpeed;
    float damage;

    public void OnInit(Vector3 target, float damage)
    {
        this.damage = damage;
        TF.forward = (target - TF.position).normalized;
    }

    private void Update() 
    {
        TF.Translate(TF.forward * moveSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other) 
    {
        IHit hit = Cache.GetIHit(other);
        if (hit != null)
        {
            hit.OnHit(damage); 
        }
        ParticlePool.Play(ParticleType.Bullet_Hit,TF.position, transform.rotation);
        OnDespawn();
    }


    public void OnDespawn()
    {
        SimplePool.Despawn(this);
    }
}
