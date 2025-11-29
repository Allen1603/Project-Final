using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager instance;

    public GameObject gameOverPanel;
    public GameObject pauseButton; // <-- ADD THIS

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // Make sure pause button is visible at start
        if (pauseButton != null)
            pauseButton.SetActive(true);
    }

    // Call this when player HP reaches 0
    public void ShowGameOver()
    {
        Time.timeScale = 0f; // Freeze game
        gameOverPanel.SetActive(true);

        if (pauseButton != null)
            pauseButton.SetActive(false); // HIDE PAUSE BUTTON
    }

    // Restart current scene
    public void RestartGame()
    {
        Time.timeScale = 1f; // Unfreeze

        if (pauseButton != null)
            pauseButton.SetActive(true); // SHOW PAUSE BUTTON

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Quit to Main Menu
    public void QuitGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
    }
}
