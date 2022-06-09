using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class InputBase : MonoBehaviour
{

    public AIState aiState { get; protected set; }
    public virtual Vector2 movementInput { get; protected set; }
    public virtual Vector2 aimPos { get; protected set; }
    public virtual Vector2 aimDir { get; protected set; }
    public virtual bool isFirePressed { get; protected set; }
    public virtual bool isAimPressed { get; protected set; }
    public virtual bool isJumpPressed { get; protected set; }

    public Action jumpPressEvent { get; set; }
    public Action jumpReleaseEvent { get; set; }

    public EntityBase self { get; protected set; }

    public abstract void EvaluateActions();
    public abstract void TimerUpdate();
    public abstract void EdgeReached();

    protected virtual void Awake()
    {
        self = GetComponent<EntityBase>();
    }

}

public enum AIState
{
    Guarding, Chasing, Returning
}
