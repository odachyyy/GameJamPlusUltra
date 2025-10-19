using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Referências")]
    [SerializeField] private UIManager uiManager;

    public int Score { get; private set; }
    public int PlayerHealth { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
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
}
