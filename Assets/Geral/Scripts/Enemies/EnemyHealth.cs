using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 1;
    private int currentHealth;
    private EnemyAI enemyAI;
    private Rigidbody2D rb;
    private Collider2D mainCollider;

    private bool isDead = false;

    private void Awake()
    {
        currentHealth = maxHealth;

        enemyAI = GetComponent<EnemyAI>();
        rb = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<Collider2D>();
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= damageAmount;
        Debug.Log("Inimigo recebeu dano! Vida atual: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Inimigo derrotado!");

        if (enemyAI != null)
        {
            enemyAI.enabled = false;
        }

        if (mainCollider != null)
        {
            mainCollider.enabled = false;
        }

        DamageZone damageZone = GetComponent<DamageZone>();
        if (damageZone != null)
        {
            damageZone.enabled = false;
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        Destroy(gameObject, 0.5f);
    }
}
