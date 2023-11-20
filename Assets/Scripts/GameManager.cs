using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int world { get; private set; }
    public int stage { get; private set; }
    public int lives { get; private set; }
    public int coins { get; private set; }
    public int score;

    public AudioClip coinSound;
    public AudioClip addLifeSound;
    public AudioClip gameOverSound;

    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        NewGame();
    }

    private void NewGame()
    {
        ResetPlayerState();
        LoadLevel(1, 1);
        AudioManager.Instance.ResumeMusic();
    }

    private void OnDestroy()
    {
        if (Instance == this) {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Instance = null;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AudioManager.Instance.PlayMusicFromStart();
    }

    public void LoadLevel(int world, int stage, bool reset = false)
    {
        this.world = world;
        this.stage = stage;

        if (reset) {
            ResetPlayerState();
        }

        SceneManager.LoadScene($"{world}-{stage}");
        UIScoreManager.Instance.UpdateUI(score, coins, world, stage, lives);
    }

    public void ResetLevel(float delay)
    {
        Invoke(nameof(ResetLevel), delay);
    }

    public void ResetLevel()
    {
        lives--;
        coins = 0;
        score = 0;

        if (lives > 0) {
            LoadLevel(world, stage);
        } else {
            GameOver();
        }

        UIScoreManager.Instance.UpdateUI(score, coins, world, stage, lives);
    }

    public void ResetPlayerState()
    {
        lives = 3;
        score = 0;
        coins = 0;

        UIScoreManager.Instance.UpdateUI(score, coins, world, stage, lives);
    }

    private void GameOver()
    {
        AudioManager.Instance.PauseMusic();
        AudioManager.Instance.PlaySFX(gameOverSound);
        Invoke(nameof(NewGame), 4f);
    }

    public void AddScore(int score)
    {
        this.score += score;
        
        UIScoreManager.Instance.UpdateUI(this.score, coins, world, stage, lives);
    }

    public void AddCoin() 
    {
        coins++;
        AddScore(200);

        if (coins == 100) {
            AddLife();
            coins = 0;
        }

        AudioManager.Instance.PlaySFX(coinSound);
        UIScoreManager.Instance.UpdateUI(score, coins, world, stage, lives);
    }

    public void AddLife()
    {
        lives++;

        AudioManager.Instance.PlaySFX(addLifeSound);
        UIScoreManager.Instance.UpdateUI(score, coins, world, stage, lives);
    }

}
