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


    protected virtual void Awake()
    {
        input = GetComponent<InputBase>();
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        gravity = new Vector3(0f, baseGravity);
        
    }

    protected virtual void Update()
    {
        input.EvaluateMovement();

        movementInput = input.movementInput;

        if (movementInput.x > 0)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        else if (movementInput.x < 0)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    protected virtual void ApplyGravity()
    {
        rb.AddForce(gravity, ForceMode.Acceleration);
    }

}
