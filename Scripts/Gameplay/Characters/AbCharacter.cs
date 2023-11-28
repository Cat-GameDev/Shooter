using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbCharacter : GameUnit
{
    public abstract void OnInit();
    public abstract void OnDeath();
    public abstract void OnDespawn();
    public abstract void OnHit(float damage);
}
