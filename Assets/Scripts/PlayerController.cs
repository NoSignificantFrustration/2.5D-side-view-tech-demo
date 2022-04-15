using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(InputBase))]
public class PlayerController : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpDuration;
    [SerializeField] private float baseGravity;
    [SerializeField] private float fallGravityMultiplier;

    private InputBase input;
    private Vector2 movementInput;
    private Rigidbody rb;
    private Vector3 gravity;
    private CapsuleCollider cc;

    private bool isGrounded = false;
    private bool isJumping = false;
    private float jumpTimeLimit;
    private float jumpBuffer = 5f;
    private float groundMemory = 5f;


    private LayerMask groundMask;
    private Vector3 groundNormalVector;

    


    private void Awake()
    {
        input = GetComponent<InputBase>();
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();
        gravity = new Vector3(0f, baseGravity);
        groundMask = LayerMask.GetMask("Ground");


        
    }


    // Start is called before the first frame update
    void Start()
    {
        input.jumpPressEvent.AddListener(JumpPressed);
        input.jumpReleaseEvent.AddListener(JumpReleased);
    }

    // Update is called once per frame
    void Update()
    {
        input.EvaluateMovement();

        movementInput = input.movementInput;
        jumpBuffer += Time.deltaTime;
        groundMemory += Time.deltaTime;

        if (movementInput.x > 0)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        else if(movementInput.x < 0)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }

        if (Physics.Raycast(new Vector3(transform.position.x - 0.25f, transform.position.y - 0.75f, 0f), Vector3.down, 0.5f, groundMask) || Physics.Raycast(new Vector3(transform.position.x + 0.25f, transform.position.y - 0.75f, 0f), Vector3.down, 0.5f, groundMask))
        {

            isGrounded = true;

            groundMemory = 0f;
            if (jumpBuffer < 0.1f)
            {
                Jump();
            }
        }
        else
        {
            isGrounded = false;

        }

        if (input.isJumpPressed && isJumping)
        {

            if (jumpTimeLimit > 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce);
                jumpTimeLimit -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

    }

    private void FixedUpdate()
    {
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - 0.75f, 0f), Vector3.down, out RaycastHit hitinfo, 0.5f, groundMask))
        {
            groundNormalVector = hitinfo.normal;
        }
        else
        {
            groundNormalVector = Vector3.up;
        }


        if (movementInput.x != 0)
        {
            rb.AddForce(new Vector3(movementInput.x * acceleration, -groundNormalVector.x, 0f), ForceMode.Impulse);
            cc.material.dynamicFriction = 0f;
            cc.material.staticFriction = 0f;
        }
        else
        {
            rb.velocity = new Vector3(rb.velocity.x / 2, rb.velocity.y, 0f);
            cc.material.dynamicFriction = 1f;
            cc.material.staticFriction = 1f;
        }

        

        if (Math.Abs(rb.velocity.x) > speed)
        {
            rb.velocity = new Vector3(speed * movementInput.x, rb.velocity.y, 0f);
        }


        if (rb.velocity.y < 0)
        {
            gravity.y = -baseGravity * fallGravityMultiplier;
        }
        else if (movementInput .x == 0 && isGrounded)
        {
            gravity.y = -baseGravity * 100f;
        }
        else
        {
            gravity.y = -baseGravity;
        }
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        rb.AddForce(gravity, ForceMode.Acceleration);
    }

    private void JumpPressed()
    {
        jumpBuffer = 0f;
        if (isGrounded || groundMemory < 0.15f)
        {
            Jump();
        }
        
    }

    private void Jump()
    {
        

        rb.velocity = new Vector3(rb.velocity.x, jumpForce);
        isJumping = true;
        jumpTimeLimit = jumpDuration;
    }

    private void JumpReleased()
    {
        isJumping = false;
        jumpTimeLimit = 0f;
    }

}
