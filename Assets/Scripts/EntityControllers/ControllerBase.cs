using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControllerBase : MonoBehaviour
{
    [field: SerializeField] public float speed { get; protected set; }
    [field: SerializeField] public float acceleration { get; protected set; }
    [field: SerializeField] public float baseGravity { get; protected set; }

    public InputBase input { get; protected set; }
    public Vector2 movementInput { get; protected set; }
    public Rigidbody rb { get; protected set; }
    public Vector3 gravity { get; protected set; }
    new public Collider collider { get; protected set; }
    public bool facingRight { get; protected set; }

    protected virtual void Awake()
    {
        SetupStats();

        input = GetComponent<InputBase>();
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        gravity = new Vector3(0f, baseGravity);
        if (transform.localRotation.y == 0)
        {
            
            facingRight = true;
        }


    }

    protected virtual void Update()
    {
        input.EvaluateActions();

        movementInput = input.movementInput;

        if (input.isAimPressed)
        {
            if (facingRight != input.aimDir.x < 0 && input.aimDir.x != 0)
            {
                Flip();
            }
        }
        else
        {
            if (facingRight != input.movementInput.x > 0 && input.movementInput.x != 0)
            {
                Flip();
            }
        }
        
    }

    protected virtual void ApplyGravity()
    {
        rb.AddForce(gravity, ForceMode.Acceleration);
    }

    protected virtual void Flip()
    {

        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
        
    }

    protected abstract void SetupStats();

}
