using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : InputBase
{
    public override Vector2 movementInput { get => controls.Player_Normal.Movement.ReadValue<Vector2>(); }
    public override Vector2 aimPos { get => aimPos; protected set => aimPos = value; }
    public override bool isJumpPressed { get => controls.Player_Normal.Jump.IsPressed(); }

    public PlayerControls controls;
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
