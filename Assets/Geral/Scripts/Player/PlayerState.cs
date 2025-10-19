using UnityEngine;

public class PlayerState : MonoBehaviour
{

    public enum EPlayerState
    {
        Idle,
        Running,
        Jumping,
        Falling
    }


    public EPlayerState CurrentState { get; private set; }

    public bool IsGrounded { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsFacingRight { get; private set; } = true; 

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckIfGrounded();

        float verticalVelocity = rb.linearVelocity.y;
        float horizontalVelocity = Mathf.Abs(rb.linearVelocity.x);

        if (IsGrounded)
        {
            if (verticalVelocity < 0.1f) 
            {
                IsJumping = false;
                if (horizontalVelocity > 0.1f)
                {
                    CurrentState = EPlayerState.Running;
                    IsRunning = true;
                }
                else
                {
                    CurrentState = EPlayerState.Idle;
                    IsRunning = false;
                }
            }
        }
        else 
        {
            IsRunning = false;
            if (verticalVelocity > 0.1f)
            {
                CurrentState = EPlayerState.Jumping;
                IsJumping = true;
            }
            else
            {
                CurrentState = EPlayerState.Falling;
            }
        }
    }

    private void CheckIfGrounded()
    {
        IsGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
    }

    public void SetFacingDirection(bool isFacingRight)
    {
        IsFacingRight = isFacingRight;
    }

    private void OnDrawGizmos()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }
    }
}
