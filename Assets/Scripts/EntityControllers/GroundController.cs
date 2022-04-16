using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : ControllerBase
{

    [SerializeField] protected float jumpForce;
    [SerializeField] protected float jumpDuration;
    [SerializeField] protected float fallGravityMultiplier;

    protected bool isGrounded = false;
    protected bool isJumping = false;
    protected float jumpTimeLimit;
    protected LayerMask groundMask;
    protected Vector3 groundNormalVector;


    protected virtual void Start()
    {
        input.jumpPressEvent.AddListener(JumpPressed);
        input.jumpReleaseEvent.AddListener(JumpReleased);
        groundMask = LayerMask.GetMask("Ground");
    }

    protected override void Update()
    {
        base.Update();
        if (movementInput.x > 0)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        else if (movementInput.x < 0)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
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

    protected virtual void FixedUpdate()
    {
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - 0.75f, 0f), Vector3.down, out RaycastHit hitinfo, 0.5f, groundMask))
        {
            groundNormalVector = hitinfo.normal;
        }
        else
        {
            groundNormalVector = Vector3.up;
        }

        CheckGround();

        if (movementInput.x != 0)
        {
            rb.AddForce(new Vector3(movementInput.x * acceleration, -groundNormalVector.x, 0f), ForceMode.Impulse);
            collider.material.dynamicFriction = 0f;
            collider.material.staticFriction = 0f;
        }
        else
        {
            rb.velocity = new Vector3(rb.velocity.x / 2, rb.velocity.y, 0f);
            collider.material.dynamicFriction = 1f;
            collider.material.staticFriction = 1f;
        }



        if (Math.Abs(rb.velocity.x) > speed)
        {
            rb.velocity = new Vector3(speed * movementInput.x, rb.velocity.y, 0f);
        }


        if (rb.velocity.y < 0)
        {
            gravity.y = -baseGravity * fallGravityMultiplier;
        }
        else if (movementInput.x == 0 && isGrounded && !input.isJumpPressed)
        {
            gravity.y = -baseGravity * 100f;
        }
        else
        {
            gravity.y = -baseGravity;
        }
        ApplyGravity();
    }

    protected virtual bool CheckGround()
    {
        if (Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y - 0.8f), 0.4f, groundMask))
        {
            isGrounded = true;
            return true;
        }
        else
        {
            isGrounded = false;
            return false;
        }
    }

    protected virtual void JumpPressed()
    {
        if (isGrounded)
        {
            Jump();
        }

    }

    protected virtual void Jump()
    {


        rb.velocity = new Vector3(rb.velocity.x, jumpForce);
        isJumping = true;
        jumpTimeLimit = jumpDuration;
    }

    protected virtual void JumpReleased()
    {
        isJumping = false;
        jumpTimeLimit = 0f;
    }

    protected virtual void OnDisable()
    {
        input.jumpPressEvent.RemoveListener(JumpPressed);
        input.jumpReleaseEvent.RemoveListener(JumpReleased);
    }
}
