using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIBase : InputBase
{

    [field: SerializeField] public virtual Vector2 targetPos { get; protected set; }
    [field: SerializeField] public virtual Transform targetEntity { get; protected set; }

}
