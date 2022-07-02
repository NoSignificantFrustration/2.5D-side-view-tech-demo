using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float damage;
    public float timeToLive;
    public Faction faction;
    private int projectileLayerIndex;
    

    protected virtual void Awake()
    {
        projectileLayerIndex = LayerMask.NameToLayer("Projectile");
        
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.layer + " " + projectileLayerIndex);
        if (other.gameObject.layer == projectileLayerIndex)
        {
            return;
        }


        if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            if (other.gameObject.TryGetComponent<EntityBase>(out EntityBase entity))
            {
                if (entity.faction == faction)
                {
                    return;
                }
            }
            damageable.Damage(damage);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    
}
