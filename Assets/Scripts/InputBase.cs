using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class InputBase : MonoBehaviour
{

    public abstract Vector2 movementInput { get; }
    public abstract Vector2 aimPos { get; protected set; }
    public abstract bool isJumpPressed { get; }

    public UnityEvent jumpPressEvent { get; protected set; }
    public UnityEvent jumpReleaseEvent{ get; protected set; }

    public abstract void EvaluateMovement();

}
