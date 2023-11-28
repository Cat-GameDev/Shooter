using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    [SerializeField] Enemy enemy;

    private void OnTriggerEnter(Collider other) 
    {
        IHit hit = Cache.GetIHit(other);
        if (hit != null)
        {
            hit.OnHit(enemy.Damage); 
        }

        if (enemy is Enemy_Explode)
        {
            enemy.OnDeath();
        }
    }
}
