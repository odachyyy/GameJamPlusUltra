using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform spriteTransform; 

    private Rigidbody2D rb;
    private bool isFacingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetHorizontalMovement(float speed)
    {
        rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
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
