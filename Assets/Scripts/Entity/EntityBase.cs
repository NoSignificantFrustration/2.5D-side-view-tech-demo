using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBase : MonoBehaviour, IDamageable
{
    [field: SerializeField] public virtual bool isTargetable { get; protected set; }
    [field: SerializeField] public virtual Faction faction { get; protected set; }
    [field: SerializeField] public virtual float health { get; protected set; }
    [field: SerializeField] public virtual float maxHealth { get; protected set; }
    public Action<float> healthChangedEvent { get; set; }

    protected virtual void OnEnable()
    {
        health = maxHealth;
    }

    public virtual void Damage(float damage)
    {
        health -= damage;
        healthChangedEvent?.Invoke(health);
    }
}


public enum Faction
{
    Player, Enemy
}
