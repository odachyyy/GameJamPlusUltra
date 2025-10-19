using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public bool JumpInputTriggered { get; private set; }
    public bool AttackInputTriggered { get; private set; }
    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Player.Enable();

        controls.Player.Move.performed += OnMove;
        controls.Player.Move.canceled += OnMove;

        controls.Player.Jump.performed += OnJump;
        controls.Player.Jump.canceled += OnJumpCanceled;

        controls.Player.Attack.performed += OnAttack;
    }

    private void OnDisable()
    {
        controls.Player.Disable();

        controls.Player.Move.performed -= OnMove;
        controls.Player.Move.canceled -= OnMove;
        
        controls.Player.Jump.performed -= OnJump;
        controls.Player.Jump.canceled -= OnJumpCanceled;

        controls.Player.Attack.performed -= OnAttack;
    }
    
    private void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    
    private void OnJump(InputAction.CallbackContext context)
    {
        JumpInputTriggered = true;
    }

    private void OnJumpCanceled(InputAction.CallbackContext context)
    {
        JumpInputTriggered = false;
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        AttackInputTriggered = true;
    }

    public void UseAttackInput()
    {
        AttackInputTriggered = false;
    }
    
    public void UseJumpInput()
    {
        JumpInputTriggered = false;
    }

}
