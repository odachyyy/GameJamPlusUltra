using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FlyingEnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform spriteTransform; 
    private Rigidbody2D rb;
    private bool isFacingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetVelocity(Vector2 velocity)
    {
        rb.linearVelocity = velocity;
    }

    public void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = spriteTransform.localScale;
        localScale.x *= -1;
        spriteTransform.localScale = localScale;
    }

    public bool IsFacingRight()
    {
        return isFacingRight;
    }
}
