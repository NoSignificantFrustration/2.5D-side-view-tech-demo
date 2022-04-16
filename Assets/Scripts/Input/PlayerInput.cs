using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : InputBase
{
    public override Vector2 movementInput { get => controls.Player_Normal.Movement.ReadValue<Vector2>(); set => movementInput = value; }
    public override Vector2 aimPos { get => aimPos; set => aimPos = value; }
    public override bool isJumpPressed { get => controls.Player_Normal.Jump.IsPressed(); set => isJumpPressed = value; }

    public PlayerControls controls { get; protected set; }

    
    protected float jumpBuffer = 5f;
    protected float groundMemory = 5f;


    

    private void Awake()
    {
        controls = new PlayerControls();
        jumpPressEvent = new UnityEngine.Events.UnityEvent();
        jumpReleaseEvent = new UnityEngine.Events.UnityEvent();
        controls.Player_Normal.Jump.started += SpacePressed;
        controls.Player_Normal.Jump.canceled += SpaceReleased;
    }

    private void SpacePressed(InputAction.CallbackContext obj)
    {
        jumpPressEvent?.Invoke();

    }

    private void SpaceReleased(InputAction.CallbackContext obj)
    {
        jumpReleaseEvent?.Invoke();

    }

    public override void EvaluateMovement()
    {
        jumpBuffer += Time.deltaTime;
        groundMemory += Time.deltaTime;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
