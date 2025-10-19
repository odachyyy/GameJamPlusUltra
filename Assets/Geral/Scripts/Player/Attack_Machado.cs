using UnityEngine;
using System.Collections;

public class Attack_Machado : MonoBehaviour, IAttackBehavior
{
    [SerializeField] private GameObject axeHitbox; 
    [SerializeField] private float attackDuration = 0.2f;
    
    private Coroutine attackCoroutine; 

    public void Activate()
    {
        if(axeHitbox != null)
            axeHitbox.SetActive(false);
    }

    public void Deactivate()
    {
        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);
        if(axeHitbox != null)
            axeHitbox.SetActive(false);
    }

    public void ExecuteAttack()
    {
        attackCoroutine = StartCoroutine(HitboxRoutine());
    }
    
    private IEnumerator HitboxRoutine()
    {
        if(axeHitbox == null) yield break;
        
        axeHitbox.SetActive(true);
        yield return new WaitForSeconds(attackDuration);
        axeHitbox.SetActive(false);
    }
}
