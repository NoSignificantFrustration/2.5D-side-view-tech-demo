using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : InputBase
{
    public override Vector2 movementInput { get => controls.Player_Normal.Movement.ReadValue<Vector2>(); protected set => movementInput = value; }
    public override bool isJumpPressed { get => controls.Player_Normal.Jump.IsPressed(); protected set => base.isJumpPressed = value; }
    public override bool isAimPressed { get => controls.Player_Normal.RMB.IsPressed(); protected set => base.isAimPressed = value; }
    public override bool isFirePressed { get => controls.Player_Normal.LMB.IsPressed(); protected set => base.isFirePressed = value; }

    public PlayerControls controls { get; protected set; }

    
    protected float jumpBuffer = 5f;
    protected float groundMemory = 5f;


    

    protected override void Awake()
    {
        base.Awake();
        controls = new PlayerControls();
        controls.Player_Normal.Jump.started += SpacePressed;
        controls.Player_Normal.Jump.canceled += SpaceReleased;
    }

    protected virtual void Start()
    {
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 15;
    }

    private void SpacePressed(InputAction.CallbackContext obj)
    {
        jumpPressEvent?.Invoke();

    }

    private void SpaceReleased(InputAction.CallbackContext obj)
    {
        jumpReleaseEvent?.Invoke();

    }

    public override void EvaluateActions()
    {
        jumpBuffer += Time.deltaTime;
        groundMemory += Time.deltaTime;
        Vector2 mouseScreenPos = controls.Player_Normal.MouseMovement.ReadValue<Vector2>();
        aimPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, -Camera.main.transform.position.z));
        aimDir = ((Vector2)transform.position - aimPos).normalized;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    public override void TimerUpdate()
    {
        
    }

    public override void EdgeReached()
    {
        
    }

    public override void EvaluateMovement()
    {
        
    }
}
