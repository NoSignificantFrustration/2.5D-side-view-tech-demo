using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//[RequireComponent(typeof(InputBase))]
public class PlayerController : GroundController
{
    [SerializeField] private GameObject weaponPivot;
    [SerializeField] private RangedWeapon rangedWeapon;

    private float jumpBuffer = 5f;
    private float groundMemory = 5f;

    protected override void Awake()
    {
        base.Awake();
        rangedWeapon.self = input.self;
    }


    // Update is called once per frame
    protected override void Update()
    {
        jumpBuffer += Time.deltaTime;
        groundMemory += Time.deltaTime;
        base.Update();
        if (input.isAimPressed)
        {
            //weapon.transform.rotation = Quaternion.EulerAngles
            //weapon.transform.rotation = Quaternion.Euler(0f, transform.rotation.y, Vector2.Angle(Vector2.left, input.aimDir));
            weaponPivot.transform.localRotation = Quaternion.FromToRotation(transform.right.x > 0 ? Vector2.left : Vector2.right, transform.right.x > 0 ? input.aimDir : new Vector2(input.aimDir.x, input.aimDir.y * -1));
            //weapon.transform.right = input.aimDir;
            //weapon.transform.localRotation = Quaternion.LookRotation(input.aimDir);
            //weapon.transform.eulerAngles = new Vector3(0, 0, Vector3.Angle(transform.right, input.aimDir));
            //weapon.transform.rotation = Quaternion.FromToRotation(Vector3.up, input.aimDir);
            if (input.isFirePressed)
            {
                rangedWeapon.Attack();
            }

        }
        else
        {
            weaponPivot.transform.rotation = transform.rotation;
        }
        
        
        //Debug.Log(input.aimDir);
        //Debug.Log(weapon.transform.right);
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

    protected override void Flip()
    {
        base.Flip();
        weaponPivot.transform.Rotate(0f, 180f, 0f);
    }
}
