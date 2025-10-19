using UnityEngine;
using System.Collections.Generic;

public class AttackHitbox : MonoBehaviour
{
    [Header("Configurações de Knockback")]
    [SerializeField] private float knockbackForce = 15f;
    [SerializeField] private float knockbackUpwardForce = 5f;

    private List<Collider2D> targetsHitThisSwing;
    private Transform playerTransform;

    private void Awake()
    {
        playerTransform = transform.root; 
    }

    private void OnEnable()
    {
        targetsHitThisSwing = new List<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (targetsHitThisSwing.Contains(other))
        {
            return;
        }

        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();

        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(1);
            
            targetsHitThisSwing.Add(other);

            Rigidbody2D enemyRb = other.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                ApplyKnockback(enemyRb);
            }
        }
    }

    private void ApplyKnockback(Rigidbody2D enemyRb)
    {
        enemyRb.linearVelocity = Vector2.zero;

        float direction = Mathf.Sign(enemyRb.transform.position.x - playerTransform.position.x);

        Vector2 force = new Vector2(direction * knockbackForce, knockbackUpwardForce);
        enemyRb.AddForce(force, ForceMode2D.Impulse);
    }
}
