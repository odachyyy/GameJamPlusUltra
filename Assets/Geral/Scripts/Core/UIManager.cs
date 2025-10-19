using UnityEngine;
using UnityEngine.UI;     
using TMPro;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [Header("Elementos da UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private List<Image> healthIcons; 

    public void UpdateScore(int newScore)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + newScore.ToString();
        }
    }

    public void UpdateHealth(int currentHealth)
    {
        for (int i = 0; i < healthIcons.Count; i++)
        {
            if (i < currentHealth)
            {
                healthIcons[i].enabled = true;
            }
            else
            {
                healthIcons[i].enabled = false;
            }
        }
    }
}
