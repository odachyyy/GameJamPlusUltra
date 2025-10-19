using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput), typeof(PlayerState))]
public class PlayerJump : MonoBehaviour
{
    [SerializeField] private float jumpForce = 15f;
    // [SerializeField] private float coyoteTime = 0.1f; // Para pulos mais responsivos
    // [SerializeField] private float jumpBufferTime = 0.1f; // Para pulos mais responsivos

    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private PlayerState playerState;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        playerState = GetComponent<PlayerState>();
    }

    private void Update()
    {
        HandleJump();
    }

    private void HandleJump()
    {

        if (playerInput.JumpInputTriggered && playerState.IsGrounded)
        {
            
            playerInput.UseJumpInput();

            
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
