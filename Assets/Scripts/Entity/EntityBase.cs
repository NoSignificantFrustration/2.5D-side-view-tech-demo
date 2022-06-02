using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBase : MonoBehaviour
{
    [field: SerializeField] public virtual bool isTargetable { get; protected set; }
    [field: SerializeField] public virtual Faction faction { get; protected set; }
    [field: SerializeField] public virtual float health { get; protected set; }
    [field: SerializeField] public virtual float maxHealth { get; protected set; }


}


public enum Faction
{
    Player, Enemy
}
