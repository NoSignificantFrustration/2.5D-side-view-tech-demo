using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void Damage(Damage damage);
}

public class Damage
{
    public float damage;
    public GameObject source; 
    public EntityBase causer;

    public Damage(float damage, GameObject source, EntityBase causer)
    {
        this.damage = damage;
        this.source = source;
        this.causer = causer;
    }
}