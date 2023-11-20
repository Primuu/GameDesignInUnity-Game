using UnityEngine;
using TMPro;

public class UIScoreManager : MonoBehaviour
{
    public static UIScoreManager Instance { get; private set; }

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI worldText;
    public TextMeshProUGUI livesText;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void UpdateUI(int score, int coins, int world, int stage, int lives)
    {
        if (scoreText != null)
            scoreText.text = "Mario " + score.ToString("000000");

        if (coinsText != null)
            coinsText.text = "x" + coins.ToString("00");

        if (worldText != null)
            worldText.text = "World " + world + " - " + stage;

        if (livesText != null)
            livesText.text = "Lives " + lives.ToString("00");
    }
}
