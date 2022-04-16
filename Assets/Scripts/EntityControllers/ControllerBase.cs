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
    }

    protected virtual void ApplyGravity()
    {
        rb.AddForce(gravity, ForceMode.Acceleration);
    }

}
