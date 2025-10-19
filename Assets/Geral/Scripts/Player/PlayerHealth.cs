using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Valores de Vida")]
    [SerializeField] private int maxHealth = 3;

    [Header("Invencibilidade")]
    [SerializeField] private float invincibilityDuration = 1f;
    
    [Header("Knockback")]
    [SerializeField] private float knockbackForce = 12f;
    [SerializeField] private float knockbackUpwardForce = 5f; 
    [SerializeField] private float knockbackDuration = 0.2f;

    private int currentHealth;
    private bool canTakeDamage = true; 
    private SpriteRenderer spriteRenderer; 
    private bool isDead = false;

    private PlayerMovement playerMovement;

    private void Awake()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdatePlayerHealth(currentHealth);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        HandleDamage(damageAmount, null);
    }

    public void TakeDamage(int damageAmount, GameObject damageSource)
    {
        HandleDamage(damageAmount, damageSource);
    }

    private void HandleDamage(int damageAmount, GameObject damageSource)
    {
        if (!canTakeDamage || isDead)
        {
            return; 
        }

        if (currentHealth <= 0)
        {
            return; 
        }

        currentHealth -= damageAmount;
        Debug.Log("Dano recebido! Vida atual: " + currentHealth);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdatePlayerHealth(currentHealth);
        }

        if (damageSource != null && playerMovement != null)
        {
            Vector3 sourcePosition = damageSource.transform.position;
            playerMovement.ApplyKnockback(sourcePosition, knockbackForce, knockbackUpwardForce, knockbackDuration);
        }

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityCooldown());
        }
    }

    private IEnumerator InvincibilityCooldown()
    {
        canTakeDamage = false;

        float endTime = Time.time + invincibilityDuration;
        while (Time.time < endTime)
        {
            if (isDead)
            {
                yield break;
            }

            spriteRenderer.enabled = !spriteRenderer.enabled; 
            yield return new WaitForSeconds(0.1f); 
        }

        if (!isDead)
        {
            spriteRenderer.enabled = true;
            canTakeDamage = true;
        }
    }

    public void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;
        canTakeDamage = false; 
        currentHealth = 0; 

        Debug.Log("O JOGADOR MORREU.");

        StopAllCoroutines();
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdatePlayerHealth(0);
        }

        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
        
        
    }
}