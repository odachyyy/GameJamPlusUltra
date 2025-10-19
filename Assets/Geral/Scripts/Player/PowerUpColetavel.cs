using UnityEngine;
using System.Collections.Generic;

public class PowerUpColetavel : MonoBehaviour
{
    public enum AttackType
    {
        None,
        Padrao,
        Machado,
        Random
    }

    [Header("Configuração do Item")]
    [Tooltip("Qual tipo de ataque este item específico concede ao jogador?")]
    [SerializeField] private AttackType attackTypeToGrant;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerAttack playerAttack = other.GetComponent<PlayerAttack>();
            if (playerAttack != null)
            {
                PowerUpColetavel.AttackType finalAttackType = attackTypeToGrant;

                if (attackTypeToGrant == AttackType.Random)
                {
                    List<PowerUpColetavel.AttackType> weaponPool = new List<PowerUpColetavel.AttackType>
                    {
                        PowerUpColetavel.AttackType.Padrao,
                        PowerUpColetavel.AttackType.Machado
                    };
                    
                    int randomIndex = Random.Range(0, weaponPool.Count);
                    finalAttackType = weaponPool[randomIndex];
                }
                playerAttack.ChangeAttackType(finalAttackType);
                
                
                Destroy(gameObject);
            }
        }
    }
}
