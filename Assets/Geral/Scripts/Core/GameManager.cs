using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Referências")]
    [SerializeField] private UIManager uiManager;

    public int Score { get; private set; }
    public int PlayerHealth { get; private set; }

    public PowerUpColetavel.AttackType CurrentPlayerAttack { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            gameObject.SetActive(false); 

            Destroy(gameObject); 
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

          
            CurrentPlayerAttack = PowerUpColetavel.AttackType.None;
            
        }
    }

    private void Start()
    {
        uiManager.UpdateScore(Score);
    }


    public void AddScore(int amount)
    {
        Score += amount;
        uiManager.UpdateScore(Score);
    }

    public void UpdatePlayerHealth(int newHealth)
    {
        PlayerHealth = newHealth;
        uiManager.UpdateHealth(PlayerHealth);
    }

    public void SetPlayerAttack(PowerUpColetavel.AttackType newType)
    {
        CurrentPlayerAttack = newType;
    }
}

