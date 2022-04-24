using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControllerBase : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected float acceleration;
    [SerializeField] protected float baseGravity;

    protected InputBase input;
    protected Vector2 movementInput;
    protected Rigidbody rb;
    protected Vector3 gravity;
    new protected Collider collider;
    protected bool facingRight;

    protected virtual void Awake()
    {
        input = GetComponent<InputBase>();
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        gravity = new Vector3(0f, baseGravity);
        if (transform.localRotation.y != 0)
        {
            //This looks counterintuitive, but it solves the problem
            facingRight = true;
        }
    }

    protected virtual void Update()
    {
        input.EvaluateMovement();

        movementInput = input.movementInput;

        if (input.isAimPressed)
        {
            if (facingRight != input.aimDir.x > 0)
            {
                Flip();
            }
        }
        else
        {
            if (facingRight != input.movementInput.x < 0 && input.movementInput.x != 0)
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

}
