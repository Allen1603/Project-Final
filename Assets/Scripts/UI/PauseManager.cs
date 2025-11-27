using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject pausePanel;

    private bool isPaused = false;

    void Start()
    {
        pausePanel.SetActive(false); 
        Time.timeScale = 1f;        
    }
 
    public void PauseGame()
    {
        if (isPaused) return;

        isPaused = true;
        pausePanel.SetActive(true); 
        Time.timeScale = 0f;        
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;         
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;  
        SceneManager.LoadScene("MainMenu");
    }
}
