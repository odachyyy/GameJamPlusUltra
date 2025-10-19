using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerInput))]
public class PlayerAttack : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private GameObject attackHitbox;

    [Header("Configurações do Ataque")]
    [SerializeField] private float attackDuration = 0.15f;
    [SerializeField] private float attackCooldown = 0.5f;

    private PlayerInput playerInput;
    private bool canAttack = true;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        attackHitbox.SetActive(false);
    }

    private void Update()
    {
        if (playerInput.AttackInputTriggered && canAttack)
        {
            playerInput.UseAttackInput();

            StartCoroutine(AttackRoutine());
        }
    }

    private IEnumerator AttackRoutine()
    {
        canAttack = false;

        attackHitbox.SetActive(true);

        yield return new WaitForSeconds(attackDuration);

        attackHitbox.SetActive(false);

        yield return new WaitForSeconds(attackCooldown - attackDuration);

        canAttack = true;
    }
}
