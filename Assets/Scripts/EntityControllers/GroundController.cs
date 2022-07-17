using System;
using UnityEngine;
using UnityEngine.Events;

public class GroundController : ControllerBase
{
    [field: SerializeField] public GroundControllerStats baseStats { get; protected set; }
    [SerializeField] protected float jumpForce;
    [SerializeField] protected float jumpDuration;
    [SerializeField] protected float fallGravityMultiplier;
    [SerializeField, Range(0.5f, 1f)] protected float idleSpeedMultiplier;

    public bool isGrounded { get; protected set; }
    public Action groundReachedEvent { get; set; }
    public bool isJumping { get; protected set; }
    public float jumpTimeLimit { get; protected set; }
    public LayerMask groundMask { get; protected set; }
    protected Vector3 groundNormalVector;

    protected override void Awake()
    {
        base.Awake();



        isGrounded = true;
        isJumping = false;
        jumpTimeLimit = 0f;
    }

    protected override void SetupStats()
    {
        speed = baseStats.speed;
        acceleration = baseStats.acceleration;
        baseGravity = baseStats.baseGravity;
        jumpForce = baseStats.jumpForce;
        jumpDuration = baseStats.jumpDuration;
        fallGravityMultiplier = baseStats.fallGravityMultiplier;
    }

    protected virtual void Start()
    {
        input.jumpPressEvent += JumpPressed;
        input.jumpReleaseEvent += JumpReleased;
        groundMask = LayerMask.GetMask("Ground");
    }

    protected override void Update()
    {
        base.Update();

        
        if (input.isJumpPressed)
        {

            if (jumpTimeLimit > 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce);
                jumpTimeLimit -= Time.deltaTime;
            }
            
        }
        
    }

    protected virtual void FixedUpdate()
    {
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - 0.75f, 0f), Vector3.down, out RaycastHit hitinfo, 1f, groundMask))
        {
            groundNormalVector = hitinfo.normal;
        }
        else
        {
            groundNormalVector = Vector3.up;
        }

        CheckGround();

        if (movementInput.x != 0f)
        {
            if (CheckLedge(movementInput.x))
            {
                input.EdgeReached();
                movementInput = input.movementInput;
            }
        }

        if (movementInput.x != 0f)
        {

            rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            rb.AddForce(new Vector3(movementInput.x * acceleration, -groundNormalVector.x, 0f), ForceMode.Impulse);
            //collider.material.dynamicFriction = 0f;
            //collider.material.staticFriction = 0f;
            //collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
        }
        else
        {
            if (isGrounded)
            {
                rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
            }
            else
            {
                rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            }
            
            //rb.velocity = new Vector3(rb.velocity.x / 2, rb.velocity.y, 0f);
            //collider.material.dynamicFriction = 1f;
            //collider.material.staticFriction = 1f;
            //collider.material.frictionCombine = PhysicMaterialCombine.Maximum;
        }



        if (Math.Abs(rb.velocity.x) > speed * movementInput.x)
        {
            if (movementInput.x == 0f)
            {
                rb.velocity = new Vector3(rb.velocity.x * idleSpeedMultiplier, rb.velocity.y, 0f);
            }
            else
            {
                rb.velocity = new Vector3(speed * movementInput.x, rb.velocity.y, 0f);
            }
            
        }


        if (rb.velocity.y < 0)
        {
            gravity = new Vector3(gravity.x, -baseGravity * fallGravityMultiplier, gravity.z);
        }
        else if (movementInput.x == 0 && isGrounded && !input.isJumpPressed)
        {
            gravity = new Vector3(gravity.x, -baseGravity * 100f, gravity.z);
        }
        else
        {
            gravity = new Vector3(gravity.x, -baseGravity, gravity.z);
        }
        ApplyGravity();
    }

    protected virtual bool CheckGround()
    {
        if (Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y - 0.8f), 0.4f, groundMask))
        {
            if (!isGrounded)
            {
                isJumping = false;
                groundReachedEvent?.Invoke();
                //Debug.Log(Time.deltaTime + " Ground reached");
            }
            isGrounded = true;
            return true;
        }
        else
        {
           
            
            isGrounded = false;
            return false;
        }
    }

    public virtual bool CheckLedge(float xDirection)
    {
        if (xDirection == 0 || !isGrounded)
        {
            return false;
        }

        if (xDirection > 0)
        {
            return !Physics.Raycast(new Vector3(transform.position.x + 1f, transform.position.y + 1f, transform.position.z), Vector3.down, 4f, groundMask);

        }
        else
        {
            return !Physics.Raycast(new Vector3(transform.position.x - 1f, transform.position.y + 1f, transform.position.z), Vector3.down, 4f, groundMask);
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
        
        jumpTimeLimit = 0f;
    }

    protected virtual void OnDisable()
    {
        input.jumpPressEvent -= JumpPressed;
        input.jumpReleaseEvent -= JumpReleased;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawRay(new Vector3(transform.position.x - 1f, transform.position.y + 1f, transform.position.z), Vector3.down * 4);
    //}
}
