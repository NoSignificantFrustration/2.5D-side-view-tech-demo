using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class InputBase : MonoBehaviour
{

    public abstract Vector2 movementInput { get; set; }
    public abstract Vector2 aimPos { get; set; }
    public abstract bool isJumpPressed { get; set; }

    public UnityEvent jumpPressEvent { get; protected set; }
    public UnityEvent jumpReleaseEvent{ get; protected set; }

    public abstract void EvaluateMovement();

}
