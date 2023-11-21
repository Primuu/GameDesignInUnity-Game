using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    private bool play = false;
    public TextMeshProUGUI playButtonText;
    public GameObject menuPanel;
    public GameObject restartButton;

    public bool isMenuActive => menuPanel.activeSelf;

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) {
            Instance = null;
        }
    }

    private void Start()
    {
        Time.timeScale = 0f;
        playButtonText.text = "PLAY";
        menuPanel.SetActive(true);
        
        restartButton.SetActive(false);
    }

    private void Update()
    {
        if (play && Input.GetKeyDown(KeyCode.Escape)) {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        menuPanel.SetActive(!menuPanel.activeSelf);
        restartButton.SetActive(play);

        if (menuPanel.activeSelf) {
            Time.timeScale = 0f;
            AudioManager.Instance.PauseMusic();
        } else {
            Time.timeScale = 1f;
            AudioManager.Instance.ResumeMusic();
        }
    }

    public void ResumeGame()
    {
        menuPanel.SetActive(false);
        Time.timeScale = 1f;

        if (!play) {
            AudioManager.Instance.PlayTheme();
        }
        
        play = true;
        playButtonText.text = "RESUME";
        restartButton.SetActive(play);
        AudioManager.Instance.ResumeMusic();
    }

    public void RestartGame()
    {
        GameManager.Instance.LoadLevel(1, 1, true);

        menuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

}
