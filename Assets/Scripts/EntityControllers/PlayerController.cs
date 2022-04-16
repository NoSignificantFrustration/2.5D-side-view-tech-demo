using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//[RequireComponent(typeof(InputBase))]
public class PlayerController : GroundController
{
    private float jumpBuffer = 5f;
    private float groundMemory = 5f;


    // Update is called once per frame
    protected override void Update()
    {
        jumpBuffer += Time.deltaTime;
        groundMemory += Time.deltaTime;
        base.Update();

    }

    protected override bool CheckGround()
    {
        if (base.CheckGround())
        {
            groundMemory = 0f;
            if (jumpBuffer < 0.2f && input.isJumpPressed)
            {
                Jump();
            }
            return true;
        }
        else
        {
            return false;
        }
        
    }

    protected override void JumpPressed()
    {
        jumpBuffer = 0f;
        if (isGrounded || groundMemory < 0.1f)
        {
            Jump();
        }
        
    }
}
