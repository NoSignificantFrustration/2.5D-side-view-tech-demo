using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [HideInInspector] public EntityBase self;
    [field: SerializeField, Min(0f)] public float minAttackRange { get; protected set; }
    [field: SerializeField, Min(0f)] public float maxAttackRange { get; protected set; }
    [field: SerializeField, Range(0f, 1f)] public float preferredAttackRange { get; protected set; }
    [field: SerializeField] public bool showDebug { get; protected set; }
    [field: SerializeField, Min(0f)] public float attackDamage { get; protected set; }
    [field: SerializeField, Min(0f)] public float attackCooldown { get; protected set; }
    public float _attackCooldown { get; protected set; }


    protected virtual void Update()
    {
        _attackCooldown -= Time.deltaTime;
    }

    protected virtual void Awake()
    {

    }

    protected virtual void SetupStats()
    {

    }

    protected virtual void OnDrawGizmosSelected()
    {
        if (!showDebug) return;

        Vector3 center = transform.position;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, minAttackRange);
        Gizmos.DrawWireSphere(center, maxAttackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(center, Mathf.Lerp(minAttackRange, maxAttackRange, preferredAttackRange));

    }

    public abstract void Attack();
}
