using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : WeaponBase
{
    [field: SerializeField, Min(0f)] public float projectileSpeed { get; protected set; }
    [SerializeField] private GameObject bullet;
    

    public override void Attack()
    {
        if (_attackCooldown > 0)
        {
            return;
        }
        _attackCooldown = attackCooldown;
        //Debug.DrawRay(transform.position, transform.right * maxAttackRange, Color.red);    
        GameObject bulletObject = Instantiate(bullet, transform.position, new Quaternion());
        bulletObject.transform.right = (Vector2)transform.right;
        bulletObject.GetComponent<Rigidbody>().velocity = bulletObject.transform.right * projectileSpeed;
        Projectile projectile = bulletObject.GetComponent<Projectile>();

        Damage damage = new Damage(attackDamage, bulletObject, self);

        projectile.damage = damage;
        projectile.timeToLive = 5f;
        projectile.faction = self.faction;
    }
}
