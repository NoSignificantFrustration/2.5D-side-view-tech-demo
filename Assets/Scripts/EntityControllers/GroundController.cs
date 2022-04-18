using System;
using UnityEngine;
using UnityEngine.Events;

public class GroundController : ControllerBase
{

    [SerializeField] protected float jumpForce;
    [SerializeField] protected float jumpDuration;
    [SerializeField] protected float fallGravityMultiplier;

    public bool isGrounded { get; protected set; }
    public UnityEvent groundReachedEvent { get; protected set; }
    public bool isJumping { get; protected set; }
    public float jumpTimeLimit { get; protected set; }
    protected LayerMask groundMask;
    protected Vector3 groundNormalVector;

    protected override void Awake()
    {
        base.Awake();
        groundReachedEvent = new UnityEvent();
        isGrounded = true;
        isJumping = false;
        jumpTimeLimit = 0f;
    }

    protected virtual void Start()
    {
        input.jumpPressEvent.AddListener(JumpPressed);
        input.jumpReleaseEvent.AddListener(JumpReleased);
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

        if (movementInput.x != 0)
        {
            rb.AddForce(new Vector3(movementInput.x * acceleration, -groundNormalVector.x, 0f), ForceMode.Impulse);
            collider.material.dynamicFriction = 0f;
            collider.material.staticFriction = 0f;
            collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
        }
        else
        {
            //rb.velocity = new Vector3(rb.velocity.x / 2, rb.velocity.y, 0f);
            collider.material.dynamicFriction = 1f;
            collider.material.staticFriction = 1f;
            collider.material.frictionCombine = PhysicMaterialCombine.Maximum;
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
            if (!isGrounded)
            {
                isJumping = false;
                groundReachedEvent.Invoke();
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
            return !Physics.Raycast(new Vector3(transform.position.x + 1f, transform.position.y + 1f, transform.position.z), groundNormalVector * -1, 4f, groundMask);

        }
        else
        {
            return !Physics.Raycast(new Vector3(transform.position.x - 1f, transform.position.y + 1f, transform.position.z), groundNormalVector * -1, 4f, groundMask);
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
        input.jumpPressEvent.RemoveListener(JumpPressed);
        input.jumpReleaseEvent.RemoveListener(JumpReleased);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), new Vector3(-4, -4));
    //}
}
