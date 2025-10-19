using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerInput))]
public class PlayerAttack : MonoBehaviour
{
    private PowerUpColetavel.AttackType currentAttackType;
    
    private IAttackBehavior currentAttackBehavior;

    [Header("Referências dos Comportamentos")]
    [SerializeField] private Attack_None attackNone;
    [SerializeField] private Attack_Espada attackEspada; 
    [SerializeField] private Attack_Machado attackMachado;

    [Header("Configurações Gerais de Ataque")]
    [Tooltip("Tempo de espera (em segundos) entre um ataque e outro.")]
    [SerializeField] private float attackCooldown = 0.5f;

    private PlayerInput playerInput;
    private bool canAttack = true;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        
        if (attackNone == null) 
            attackNone = GetComponent<Attack_None>();

        if (attackEspada == null) 
            attackEspada = GetComponent<Attack_Espada>();
            
        if (attackMachado == null) 
            attackMachado = GetComponent<Attack_Machado>();
        
        

        if (GameManager.Instance != null)
        {
            ChangeAttackType(GameManager.Instance.CurrentPlayerAttack);
        }
        else
        {
            ChangeAttackType(PowerUpColetavel.AttackType.None);
        }
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

        if (currentAttackBehavior != null)
        {
            currentAttackBehavior.ExecuteAttack();
        }

        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }

    public void ChangeAttackType(PowerUpColetavel.AttackType newType)
    {
        if (newType == PowerUpColetavel.AttackType.Random)
        {
             Debug.LogError("PlayerAttack recebeu um 'Random'! O Coletável devia ter sorteado.");
             return;
        }

        currentAttackType = newType;

        if(currentAttackBehavior != null)
        {
            currentAttackBehavior.Deactivate();
        }

        switch (currentAttackType)
        {
            case PowerUpColetavel.AttackType.None:
                currentAttackBehavior = attackNone;
                break;

            case PowerUpColetavel.AttackType.Padrao:
                currentAttackBehavior = attackEspada;
                break;
                
            case PowerUpColetavel.AttackType.Machado:
                currentAttackBehavior = attackMachado;
                break;
        }
        
        currentAttackBehavior.Activate();

        if (GameManager.Instance != null && 
            newType != PowerUpColetavel.AttackType.None)
        {
            GameManager.Instance.SetPlayerAttack(newType);
        }
    }
}