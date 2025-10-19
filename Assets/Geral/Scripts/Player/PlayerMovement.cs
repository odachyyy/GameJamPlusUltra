using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput), typeof(PlayerState))]
public class PlayerMovement : MonoBehaviour
{
   [Header("Configurações de Movimento")]
    [SerializeField] private float moveSpeed = 8f;

    [Header("Referências")]
    [SerializeField] private Transform spriteTransform;

    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private PlayerState playerState;

    private float moveInputX;
    private bool canMove = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        playerState = GetComponent<PlayerState>();
    }

    private void Update()
    {
        if (!canMove)
        {
            moveInputX = 0;
            return;
        }

        moveInputX = playerInput.MoveInput.x;

        HandleFlip();
    }

    private void FixedUpdate()
    {
        if (!canMove)
        {
            return;
        }

        rb.linearVelocity = new Vector2(moveInputX * moveSpeed, rb.linearVelocity.y);
    }

    private void HandleFlip()
    {
        if (spriteTransform == null) return;

        if ((moveInputX > 0 && !playerState.IsFacingRight) || (moveInputX < 0 && playerState.IsFacingRight))
        {
            playerState.SetFacingDirection(!playerState.IsFacingRight);

            Vector3 spriteScale = spriteTransform.localScale;
            spriteScale.x *= -1;
            spriteTransform.localScale = spriteScale;
        }
    }

    public void ApplyKnockback(Vector3 damageSourcePosition, float knockbackForce, float knockbackUpwardForce, float knockbackDuration)
    {
        StartCoroutine(KnockbackRoutine(damageSourcePosition, knockbackForce, knockbackUpwardForce, knockbackDuration));
    }

    private IEnumerator KnockbackRoutine(Vector3 damageSourcePosition, float force, float upwardForce, float duration)
    {
        canMove = false;

        float horizontalDirection = (transform.position.x - damageSourcePosition.x) > 0 ? 1f : -1f;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(horizontalDirection * force, upwardForce), ForceMode2D.Impulse);

        yield return new WaitForSeconds(duration);

        canMove = true;
    }
}
