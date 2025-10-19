// IAttackBehavior.cs

/// <summary>
/// Uma interface (contrato) que define o que um comportamento de ataque
/// deve ser capaz de fazer.
/// </summary>
public interface IAttackBehavior
{
    /// <summary>
    /// Chamado quando este ataque é equipado (ex: ligar UI, preparar).
    /// </summary>
    void Activate();      
    
    /// <summary>
    /// Chamado quando este ataque é desequipado (ex: desligar UI, parar coroutines).
    /// </summary>
    void Deactivate();    
    
    /// <summary>
    /// Chamado pelo PlayerAttack quando o botão de ataque é pressionado.
    /// </summary>
    void ExecuteAttack(); 
}