using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Projectile_Launch : GameUnit
{
    public const float flighHeight = 4f;
    public const float totalTime = 0.75f;
    float damage;

    public void Launch(Vector3 destination, float damage)
    {
        this.damage = damage;

        transform.DOMoveY(flighHeight, totalTime).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            transform.DOMove(destination, totalTime).SetEase(Ease.InQuad);
        });
    }


    private void OnTriggerEnter(Collider other) 
    {
        IHit hit = Cache.GetIHit(other);
        if (hit != null)
        {
            hit.OnHit(damage); 
        }
        ParticlePool.Play(ParticleType.Enemy_Launch_Explosion_Particle,TF.position, transform.rotation);
        OnDespawn();
    }

    public void OnDespawn()
    {
        SimplePool.Despawn(this);
    }

}
