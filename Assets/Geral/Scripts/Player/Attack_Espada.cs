// Attack_Espada.cs
using UnityEngine;
using System.Collections;

// Note que ele "herda" de MonoBehaviour E da nossa interface IAttackBehavior
public class Attack_Espada : MonoBehaviour, IAttackBehavior
{
    [Header("Configurações da Espada")]
    [SerializeField] private GameObject attackHitbox; // Arraste sua "AttackHitbox" padrão aqui
    [SerializeField] private float attackDuration = 0.15f;
    
    // Guarda a referência da Coroutine para podermos pará-la
    private Coroutine attackCoroutine; 

    /// <summary>
    /// Chamado pelo PlayerAttack quando a espada é equipada.
    /// </summary>
    public void Activate()
    {
        // Garante que a hitbox da espada esteja desligada
        if(attackHitbox != null)
            attackHitbox.SetActive(false);
    }

    /// <summary>
    /// Chamado pelo PlayerAttack quando a espada é desequipada.
    /// </summary>
    public void Deactivate()
    {
        // Se o jogador trocar de arma no meio de um ataque,
        // pare o ataque e desligue a hitbox.
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
        if(attackHitbox != null)
            attackHitbox.SetActive(false);
    }

    /// <summary>
    /// Chamado pelo PlayerAttack quando o botão de ataque é pressionado.
    /// </summary>
    public void ExecuteAttack()
    {
        // Inicia a rotina de ligar/desligar a hitbox
        attackCoroutine = StartCoroutine(HitboxRoutine());
    }
    
    /// <summary>
    /// Esta é a lógica que estava no seu PlayerAttack.cs antigo.
    /// </summary>
    private IEnumerator HitboxRoutine()
    {
        if(attackHitbox == null) 
            yield break; // Segurança: não faz nada se a hitbox não estiver configurada
        
        attackHitbox.SetActive(true);
        yield return new WaitForSeconds(attackDuration);
        attackHitbox.SetActive(false);
    }
}