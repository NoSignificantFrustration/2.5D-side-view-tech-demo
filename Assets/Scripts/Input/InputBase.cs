using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class InputBase : MonoBehaviour
{

    public virtual Vector2 movementInput { get; protected set; }
    public virtual Vector2 aimPos { get; protected set; }
    public virtual bool isJumpPressed { get; protected set; }

    public UnityEvent jumpPressEvent { get; protected set; }
    public UnityEvent jumpReleaseEvent{ get; protected set; }

    public abstract void EvaluateMovement();

    protected virtual void Awake()
    {
        jumpPressEvent = new UnityEngine.Events.UnityEvent();
        jumpReleaseEvent = new UnityEngine.Events.UnityEvent();
    }

}
